/proc/RunTest()
	var/alist/L = alist(11 = "z-level", 12 = "other")

	L -= 11
	ASSERT(!(11 in L))
	ASSERT(12 in L)

	L -= 99
	ASSERT(length(L) == 1)
