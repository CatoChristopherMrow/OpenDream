/proc/RunTest()
	var/library = "libc.so.6"
	var/function = "printf"
	if (world.system_type == MS_WINDOWS)
		library = "kernel32.dll"
		function = "GetTickCount"

	var/F = load_ext(library, function)
	ASSERT(!isnull(F))
	ASSERT(!istext(F))
	ASSERT(!istype(F, /datum))

	var/caught = FALSE
	try
		var/Bad = load_ext(library, "definitely_missing_opendream_symbol")
		ASSERT(isnull(Bad))
	catch
		caught = TRUE

	ASSERT(caught)
