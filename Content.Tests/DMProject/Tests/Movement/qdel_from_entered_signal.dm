//# issue 2194

/world
	maxx = 2
	maxy = 1
	maxz = 1

#define COMSIG_ATOM_ENTERED "atom_entered"
#define COMSIG_MOVABLE_MOVED "movable_moved"
#define QDELETED(thing) (isnull(thing) || thing.gc_destroyed)

/datum
	var/gc_destroyed
	var/list/_listen_lookup
	var/list/_signal_procs

/datum/proc/RegisterSignal(datum/target, signal_type, proctype)
	var/list/procs = (_signal_procs ||= list())
	var/list/target_procs = (procs[target] ||= list())
	var/list/lookup = (target._listen_lookup ||= list())

	target_procs[signal_type] = proctype
	var/list/looked_up = lookup[signal_type]
	if(isnull(looked_up))
		lookup[signal_type] = src
	else if(!islist(looked_up))
		lookup[signal_type] = list(looked_up, src)
	else
		looked_up += src

/datum/proc/_SendSignal(sigtype, list/arguments)
	var/target = _listen_lookup[sigtype]
	if(!length(target))
		var/datum/listening_datum = target
		return call(listening_datum, listening_datum._signal_procs[src][sigtype])(arglist(arguments))
	for(var/i in 1 to length(target))
		var/datum/listening_datum = target[i]
		call(listening_datum, listening_datum._signal_procs[src][sigtype])(arglist(arguments))

/proc/qdel(datum/to_delete)
	to_delete.gc_destroyed = TRUE

/atom/Entered(atom/movable/arrived, atom/old_loc)
	..()
	if(_listen_lookup)
		_SendSignal(COMSIG_ATOM_ENTERED, list(src, arrived, old_loc))

/atom/movable/Move(atom/newloc, direct)
	var/atom/oldloc = loc
	. = ..()
	if(. && _listen_lookup)
		_SendSignal(COMSIG_MOVABLE_MOVED, list(src, oldloc))

/obj/qdel_from_entered_target

/obj/qdel_from_entered_listener

/obj/qdel_from_entered_listener/proc/on_entered(datum/source, atom/movable/arrived)
	if(src == arrived)
		return
	qdel(arrived)

/obj/qdel_from_entered_listener/proc/connect_to_loc()
	RegisterSignal(src.loc, COMSIG_ATOM_ENTERED, "on_entered")

/proc/RunTest()
	world.maxx = 2
	world.maxy = 1
	world.maxz = 1

	var/turf/start = locate(1, 1, 1)
	var/turf/end = locate(2, 1, 1)
	var/obj/qdel_from_entered_target/target = new(start)
	var/obj/qdel_from_entered_listener/listener = new(end)
	ASSERT(end)
	ASSERT(listener.loc == end)
	listener.connect_to_loc()

	ASSERT(step(target, EAST))
	ASSERT(QDELETED(target))

#undef COMSIG_ATOM_ENTERED
#undef COMSIG_MOVABLE_MOVED
#undef QDELETED
