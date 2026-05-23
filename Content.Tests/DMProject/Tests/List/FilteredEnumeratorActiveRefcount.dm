// NOBYOND - OpenDream refcount regression test
/datum/filtered_active_refcount

/proc/RunTest()
	var/datum/filtered_active_refcount/kept = new
	var/list/holders = list(new /datum/filtered_active_refcount, kept, new /datum/filtered_active_refcount)

	for(var/datum/filtered_active_refcount/object in holders)
		ASSERT(refcount(kept) == 2)
		break
