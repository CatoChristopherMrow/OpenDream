// NOBYOND - OpenDream refcount regression test
/proc/RunTest()
	var/datum/kept = new
	var/list/holders = list(new /datum, kept, new /datum)

	ASSERT(refcount(kept) == 2)

	for(var/object in holders)
		break

	ASSERT(refcount(kept) == 2)
