/world
	maxx = 2
	maxy = 1
	maxz = 1

/proc/RunTest()
	world.maxx = 2
	world.maxy = 1
	world.maxz = 1

	var/turf/old_turf = locate(1, 1, 1)
	var/turf/new_turf = locate(2, 1, 1)
	var/obj/item = new(old_turf)

	ASSERT(item in old_turf.contents)
	ASSERT(!(item in new_turf.contents))

	item.loc = new_turf

	ASSERT(!(item in old_turf.contents))
	ASSERT(item in new_turf.contents)

	item.loc = null

	ASSERT(!(item in old_turf.contents))
	ASSERT(!(item in new_turf.contents))
	ASSERT(length(item.locs) == 0)

	var/obj/wide = new(old_turf)
	wide.bound_width = 64
	ASSERT(old_turf in wide.locs)
	ASSERT(new_turf in wide.locs)
