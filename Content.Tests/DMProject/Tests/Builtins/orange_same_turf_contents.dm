/proc/RunTest()
	world.maxx = 3
	world.maxy = 3
	world.maxz = 1

	var/turf/T = locate(1, 1, 1)
	var/obj/center = new(T)
	var/obj/other = new(T)

	var/list/nearby = orange(1, center)
	ASSERT(!(center in nearby))
	ASSERT(other in nearby)
	ASSERT(T in nearby)
	var/found = locate(/obj) in nearby
	ASSERT(found == other)
