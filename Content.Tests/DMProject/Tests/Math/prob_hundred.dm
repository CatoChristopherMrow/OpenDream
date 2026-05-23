/proc/RunTest()
	for(var/i in 1 to 100)
		ASSERT(prob(100))

	ASSERT(!prob(0))
