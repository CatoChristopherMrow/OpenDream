/var/list/spawn_delay_fifo_events = list()

/proc/spawn_delay_fifo_record(value)
	spawn_delay_fifo_events += value

/proc/RunTest()
	spawn(0)
		spawn_delay_fifo_record("first")

	spawn(0)
		spawn_delay_fifo_record("second")

	ASSERT(spawn_delay_fifo_events.len == 0)

	sleep(0)

	ASSERT(spawn_delay_fifo_events.len == 2)
	ASSERT(spawn_delay_fifo_events[1] == "first")
	ASSERT(spawn_delay_fifo_events[2] == "second")
