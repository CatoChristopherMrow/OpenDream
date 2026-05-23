/datum/qdel_managed_local_survives_destroy
	var/gc_destroyed
	var/preserved = "still here"

/datum/qdel_managed_local_survives_destroy/proc/Destroy()
	return 0

/proc/qdel_managed_local_survives_destroy_fake_qdel(datum/qdel_managed_local_survives_destroy/thing)
	thing.gc_destroyed = 1
	thing.Destroy()

/proc/RunTest()
	var/datum/qdel_managed_local_survives_destroy/thing = new

	qdel_managed_local_survives_destroy_fake_qdel(thing)

	ASSERT(thing.preserved == "still here")
