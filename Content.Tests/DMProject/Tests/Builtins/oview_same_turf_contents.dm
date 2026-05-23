/world
	maxx = 3
	maxy = 3
	maxz = 1

/turf
	icon = 'turf.dmi'

/proc/RunTest()
	world.maxx = 3
	world.maxy = 3
	world.maxz = 1

	var/turf/T = locate(2, 2, 1)
	var/obj/center = new(T)
	var/obj/other = new(T)

	var/list/nearby = oview(1, center)
	ASSERT(!(center in nearby))
	ASSERT(other in nearby)
	var/found = locate(/obj) in nearby
	ASSERT(found == other)
