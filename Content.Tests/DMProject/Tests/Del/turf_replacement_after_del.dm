/turf/replacement_after_del

/world
	maxx = 1
	maxy = 1
	maxz = 1

/proc/RunTest()
	world.maxx = 1
	world.maxy = 1
	world.maxz = 1

	var/turf/original = locate(1, 1, 1)
	del(original)

	ASSERT(!isnull(original))
	ASSERT(original.x == 1)
	ASSERT(original.y == 1)

	var/turf/replacement = new /turf/replacement_after_del(original)
	ASSERT(replacement == original)
	ASSERT(original.type == /turf/replacement_after_del)
