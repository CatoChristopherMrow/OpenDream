// NOBYOND
/proc/RunTest()
	var/datum/D = list()

	ASSERT(isnull(D:gc_destroyed))
	ASSERT(isnull(D:anything_else))
