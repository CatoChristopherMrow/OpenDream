/proc/RunTest()
	var/list/L = list("")
	var/atom/maybe_not_an_atom = L[1]

	ASSERT(istext(maybe_not_an_atom))

	for(var/i in 1 to length(L))
		var/atom/from_loop = L[i]
		if(istext(from_loop))
			continue

		CRASH("Empty string lost text-ness after typed assignment in a numeric list loop")
