// NOBYOND - OpenDream refcount regression test
/datum/test_object

/proc/RunTest()
	var/datum/test_object/kept = new
	var/list/holders = list(new /datum/test_object, kept, new /datum/test_object)

	ASSERT(refcount(kept) == 2)

	for(var/datum/test_object/object in holders)
		break

	ASSERT(refcount(kept) == 2)
