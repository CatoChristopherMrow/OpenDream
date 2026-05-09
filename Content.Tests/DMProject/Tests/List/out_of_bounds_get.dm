/proc/RunTest()
	var/list/L = list("first")
	ASSERT(isnull(L[0]))
	ASSERT(isnull(L[2]))
	ASSERT(isnull(L[-1]))

	L[3] = "third"
	ASSERT(L.len == 3)
	ASSERT(L[1] == "first")
	ASSERT(isnull(L[2]))
	ASSERT(L[3] == "third")
