/proc/RunTest()
	var/datum/victim = new
	var/baseline = refcount(victim)

	var/result = pick(null, victim, null, null)
	result = null

	ASSERT(refcount(victim) == baseline)
