/proc/RunTest()
	var/list/empty_targets = list()
	empty_targets << "hello"

	ASSERT(empty_targets.len == 0)
