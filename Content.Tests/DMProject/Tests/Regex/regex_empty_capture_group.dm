/proc/RunTest()
	var/regex/R = regex(@"^\s*([\W\w]*?)\s*$")

	ASSERT(R.Find("") == 1)
	ASSERT(isnull(R.group[1]))
	ASSERT(R.match == "")

	ASSERT(R.Find(" abc ") == 1)
	ASSERT(R.group[1] == "abc")
	ASSERT(R.match == " abc ")

	var/regex/star = regex(@"^(a*)$")
	ASSERT(star.Find("") == 1)
	ASSERT(isnull(star.group[1]))

	var/regex/optional = regex(@"^(a)?$")
	ASSERT(optional.Find("") == 1)
	ASSERT(isnull(optional.group[1]))
