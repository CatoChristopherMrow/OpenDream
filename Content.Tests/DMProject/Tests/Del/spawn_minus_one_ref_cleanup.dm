// NOBYOND
/datum/spawn_minus_one_ref_cleanup
	proc/noop()
		return

/proc/_spawn_minus_one_call(datum/spawn_minus_one_ref_cleanup/target)
	spawn(-1)
		target.noop()

/proc/RunTest()
	var/datum/spawn_minus_one_ref_cleanup/target = new
	ASSERT(refcount(target) == 1)
	_spawn_minus_one_call(target)
	ASSERT(refcount(target) == 1)
