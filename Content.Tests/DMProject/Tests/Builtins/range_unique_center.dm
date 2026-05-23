/obj/range_unique_center

/world
	maxx = 3
	maxy = 3
	maxz = 1

/proc/RunTest()
	world.maxx = 3
	world.maxy = 3
	world.maxz = 1

	var/turf/T = locate(1, 1, 1)
	var/obj/range_unique_center/O = new(T)
	var/list/ranged = range(1, O)
	var/found_count = 0

	for(var/atom/A as anything in ranged)
		if(A == O)
			found_count++

	ASSERT(found_count == 1)
	ASSERT(!(O in (ranged - list(O))))
