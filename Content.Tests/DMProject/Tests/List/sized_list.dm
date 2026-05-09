// NOBYOND
/proc/RunTest()
	var/list/L = new(1)
	ASSERT(L.len == 1)
	ASSERT(isnull(L[1]))

	L[1] = "first"
	ASSERT(L.len == 1)
	ASSERT(L[1] == "first")

	L[3] = "third"
	ASSERT(L.len == 3)
	ASSERT(isnull(L[2]))
	ASSERT(L[3] == "third")
