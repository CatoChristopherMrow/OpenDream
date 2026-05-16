/proc/RunTest()
	var/atom/movable/A = new /obj()

	A.filters += filter(type = "color")
	ASSERT(length(A.filters) == 1)
