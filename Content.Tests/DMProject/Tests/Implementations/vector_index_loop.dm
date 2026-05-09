/proc/RunTest()
	var/vector/two = vector(3, 4)
	ASSERT(two[1] == 3)
	ASSERT(two[2] == 4)

	var/list/seen = list()
	for (var/i in 1 to two.len)
		seen += two[i]

	ASSERT(seen.len == 2)
	ASSERT(seen[1] == 3)
	ASSERT(seen[2] == 4)

	var/vector/three = vector(5, 6, 7)
	ASSERT(three[3] == 7)

	var/sum = 0
	for (var/i in 1 to three.len)
		sum += three[i]

	ASSERT(sum == 18)

	three.x = 50
	three.y = 60
	ASSERT(three[1] == 50)
	ASSERT(three[2] == 60)
	ASSERT(three[3] == 7)
