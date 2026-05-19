// NOBYOND - OpenDream refcount regression test
/proc/RunTest()
	var/datum/target = new
	ASSERT(refcount(target) == 1)

	if (TRUE)
		var/list/scoped_holder = list(target)
		ASSERT(refcount(target) == 2)

	ASSERT(refcount(target) == 1)
