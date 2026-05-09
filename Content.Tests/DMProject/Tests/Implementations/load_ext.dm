/proc/RunTest()
	var/F = load_ext("libc.so.6", "printf")
	ASSERT(!isnull(F))
	ASSERT(!istext(F))
	ASSERT(!istype(F, /datum))

	var/caught = FALSE
	try
		var/Bad = load_ext("libc.so.6", "definitely_missing_opendream_symbol")
		ASSERT(isnull(Bad))
	catch
		caught = TRUE

	ASSERT(caught)
