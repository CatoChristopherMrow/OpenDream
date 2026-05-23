/proc/RunTest()
	var/atom/A = new /obj()
	ASSERT(isnull(A.color))
	ASSERT(isnull(initial(A.color)))

	A.color = "#ff0000"
	ASSERT(A.color == "#ff0000")
