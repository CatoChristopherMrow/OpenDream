/datum/qdel_marked_ref_drop
	var/gc_destroyed

/datum/qdel_marked_ref_drop/Del()
	CRASH("qdel-marked object was deleted by refcount cleanup")

/proc/RunTest()
	var/datum/qdel_marked_ref_drop/target = new
	target.gc_destroyed = -2
	target = null

