// NOBYOND
/proc/RunTest()
	var/datum/thing = new()
	del(thing)

	ASSERT(isnull(thing.type))
