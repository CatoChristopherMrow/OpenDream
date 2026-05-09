// NOBYOND - load_resource is OpenDream-specific
/proc/RunTest()
	text2file("hello", "load_resource_source.txt")

	var/resource = load_resource("load_resource_source.txt")
	ASSERT(isfile(resource))
	ASSERT(fcopy(resource, "load_resource_copy.txt"))
	ASSERT(file2text("load_resource_copy.txt") == "hello")

	fdel("load_resource_source.txt")
	fdel("load_resource_copy.txt")
