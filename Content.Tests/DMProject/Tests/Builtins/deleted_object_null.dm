/proc/RunTest()
	var/datum/thing = new()
	del(thing)

	ASSERT(isnull(thing))
	ASSERT("[thing]" == "")
