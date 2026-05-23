// NOBYOND - resource.hash is an OpenDream implementation smoke test
//# issue 2194

/datum/resource_hash_holder
	var/hash

/proc/RunTest()
	var/datum/resource_hash_holder/resource = file("resource_hash_test.txt")

	ASSERT(istext(resource.hash))
	ASSERT(length(resource.hash) == 32)
