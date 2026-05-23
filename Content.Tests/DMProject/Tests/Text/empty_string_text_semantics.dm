/proc/RunTest()
	ASSERT(istext(""))
	ASSERT(length("") == 0)
	ASSERT(copytext("", 1, 256) == "")
	ASSERT(regex(@"[<>]", "g").Replace("", "") == "")

	var/list/L = list("")
	for (var/value as anything in L)
		ASSERT(istext(value))
