/proc/RunTest()
	var/datum/kept = new
	var/list/holders = list(new /datum, kept, new /datum)

	for(var/object in holders)
		ASSERT(refcount(kept) == 2)
		break

