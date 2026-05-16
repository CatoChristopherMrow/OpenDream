/proc/RunTest()
	ASSERT(replacetextEx("", null, "-") == "")
	ASSERT(replacetextEx("abc", null, "-") == "a-b-c")
