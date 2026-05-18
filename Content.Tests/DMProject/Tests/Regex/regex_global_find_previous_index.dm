/proc/RunTest()
	var/regex/R = regex(@"^/obj[^\n]*$", "gm")
	var/text = "/obj/one\n/obj/two\n"
	var/match_index = R.Find(text)

	ASSERT(match_index == 1)
	ASSERT(R.match == "/obj/one")

	match_index = R.Find(text, match_index)

	ASSERT(match_index == 10)
	ASSERT(R.match == "/obj/two")

	match_index = R.Find(text, match_index)

	ASSERT(match_index == 0)
