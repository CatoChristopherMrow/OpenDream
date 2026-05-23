/proc/RunTest()
	var/regex/R = regex(@"foo", "g")

	ASSERT(R.Find_char("foo foo") == 1)
	ASSERT(R.next == 4)
	ASSERT(R.Find_char("foo foo") == 5)

	R = regex(@"foo")
	ASSERT(R.Replace_char("foo foo", "bar") == "bar foo")
	ASSERT(R.Replace_char("foo foo", "bar", 5) == "foo bar")
