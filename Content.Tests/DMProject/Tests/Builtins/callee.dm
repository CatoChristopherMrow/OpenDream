/proc/bar()
	set desc = "bar description"
	
	ASSERT(callee.name == "bar")
	ASSERT(callee.desc == "bar description")
	return callee

/proc/leaf()
	ASSERT(callee.name == "leaf")
	ASSERT(caller.name == "middle")

/proc/middle()
	ASSERT(caller.name == "RunTest")
	leaf()

/proc/RunTest()
	ASSERT(callee.name == "RunTest")
	ASSERT(copytext(callee.file, -9) == "callee.dm")

	middle()
	
	var/callee/expired_callee = bar()
	var/failed = FALSE
	try
		var/name = expired_callee.name
	catch (var/exception/E)
		failed = TRUE
	ASSERT(failed)
