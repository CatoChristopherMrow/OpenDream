using System.Threading.Tasks;

//
// A quick note about how we get timing right here, since it is sensitive. (I don't know where else to write it down.)
//
// When we complete a task via TaskCompletionSource, any awaiters will be **synchronously** completed.
// (assuming the awaiter doesn't have a synchronization context from a different thread, which we don't here.)
// We make heavy use of this.
//
// At the start of a tick, any spawn()/sleep()s that are supposed to resume this tick
// are still very much awaiting. The first thing we do in the update loop here is complete all of those (UpdateDelays).
// When we complete them, they will **synchronously** end up calling Schedule() on us here (through proc state stuff).
// And that then makes it available to us, to correctly resume the procs for real.
//
// Before I fixed this, there were some uses of task schedulers etc that all added various delays/thread pool jumping.
// The current system relies on async void, which works correctly.
//
// I don't like the fact that we're indirecting through so much complex .NET async logic for all of this,
// but ah well. Works for now.
//

namespace OpenDreamRuntime.Procs;

public sealed partial class ProcScheduler {
    private readonly HashSet<AsyncNativeProc.AsyncNativeProcState> _sleeping = new();
    private readonly HashSet<DreamThread> _delayedSpawnThreads = new();
    private readonly Queue<ScheduledWork> _scheduled = new();
    private AsyncNativeProc.AsyncNativeProcState? _current;

    public Task Schedule(AsyncNativeProc.AsyncNativeProcState state, Func<AsyncNativeProc.AsyncNativeProcState, Task<DreamValue>> taskFunc) {
        async Task Foo() {
            state.Result = await taskFunc(state);
            if (!_sleeping.Remove(state))
                return;

            _scheduled.Enqueue(new ScheduledWork(state));
        }

        var task = Foo();
        if (!task.IsCompleted) // No need to schedule the proc if it's already finished
            _sleeping.Add(state);

        return task;
    }

    public void Process() {
        UpdateDelays();

        // Update all asynchronous tasks that have finished waiting and are ready to resume.
        //
        // Note that this is in a loop with _deferredTasks.
        // If a proc calls sleep(1) or such, it gets put into _deferredTasks.
        // When we drain the _deferredTasks lists, it'll indirectly schedule things into _scheduled again.
        // This should all happen synchronously (see above).
        while (_scheduled.Count > 0 || _deferredTasks.Count > 0) {
            while (_scheduled.TryDequeue(out var work)) {
                if (work.State is { } state) {
                    _current = state;
                    state.SafeResume();
                    _current = null;
                } else if (work.Thread is { } thread) {
                    thread.Resume().Dispose();
                }
            }

            while (_deferredTasks.TryDequeue(out var task)) {
                task.TrySetResult();
            }
        }
    }

    public async void ScheduleSpawn(DreamThread thread, float delay) {
        _delayedSpawnThreads.Add(thread);

        try {
            await CreateDelay(delay);
            _delayedSpawnThreads.Remove(thread);
            _scheduled.Enqueue(new ScheduledWork(thread));
        } finally {
            _delayedSpawnThreads.Remove(thread);
        }
    }

    public IEnumerable<DreamThread> InspectThreads() {
        // TODO: We shouldn't need to check if Thread is null here
        //       I think we're keeping disposed states somewhere here

        if (_current?.Thread is not null) {
            yield return _current.Thread;
        }

        foreach (var work in _scheduled) {
            if (work.Thread is { } thread) {
                yield return thread;
            } else if (work.State?.Thread is { } stateThread) {
                yield return stateThread;
            }
        }

        foreach (var thread in _delayedSpawnThreads) {
            yield return thread;
        }
    }

    private readonly struct ScheduledWork {
        public readonly AsyncNativeProc.AsyncNativeProcState? State;
        public readonly DreamThread? Thread;

        public ScheduledWork(AsyncNativeProc.AsyncNativeProcState state) {
            State = state;
            Thread = null;
        }

        public ScheduledWork(DreamThread thread) {
            State = null;
            Thread = thread;
        }
    }
}
