/world
	maxx = 3
	maxy = 3
	maxz = 2

/proc/RunTest()
	world.maxx = 3
	world.maxy = 3
	world.maxz = 2

	ASSERT(block(-1000000, -1000000, 1, 2, 2, 1).len == 0)

	ASSERT(block(-10, -10, 1, -5, -5, 1).len == 0)
	ASSERT(block(world.maxx + 1, 1, 1, world.maxx + 5, 1, 1).len == 0)
	ASSERT(block(1, 1, world.maxz + 1, 1, 1, world.maxz + 5).len == 0)
