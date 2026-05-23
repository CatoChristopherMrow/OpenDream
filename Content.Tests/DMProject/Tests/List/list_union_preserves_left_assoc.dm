/proc/RunTest()
	var/list/left = list("a" = list(1, 2))
	var/list/right = list("b" = list(3, 4))
	var/list/union = left | right

	ASSERT(union["a"])
	ASSERT(union["a"][1] == 1)
	ASSERT(union["b"])
	ASSERT(union["b"][1] == 3)
