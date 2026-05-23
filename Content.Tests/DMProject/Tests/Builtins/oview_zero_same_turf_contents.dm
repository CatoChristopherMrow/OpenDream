/obj/oview_zero_same_turf_center

/obj/oview_zero_same_turf_other

/world
	maxx = 3
	maxy = 3
	maxz = 1

/proc/RunTest()
	world.maxx = 3
	world.maxy = 3
	world.maxz = 1

	var/turf/T = locate(2, 2, 1)
	var/obj/oview_zero_same_turf_center/center = new(T)
	var/obj/oview_zero_same_turf_other/other = new(T)

	var/list/nearby = oview(0, center)
	ASSERT(!(center in nearby))
	ASSERT(other in nearby)
