/area/contents_assignment

/proc/RunTest()
	world.maxx = 1
	world.maxy = 1
	world.maxz = 1

	var/turf/turf = locate(1, 1, 1)
	var/area/old_area = turf.loc
	var/area/contents_assignment/new_area = new

	new_area.contents = list(turf)

	ASSERT(turf.loc == new_area)
	ASSERT(turf in new_area.contents)
	ASSERT(!(turf in old_area.contents))
