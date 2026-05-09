/world
	maxx = 1
	maxy = 1
	maxz = 1

/proc/RunTest()
	var/obj/object = new(locate(1, 1, 1))

	var/list/inside = bounds(object, 0)
	ASSERT(object in inside)

	var/list/outside = obounds(object, 0)
	ASSERT(!(object in outside))

	object.step_x = 2
	object.step_y = 3
	object.bound_x = 1
	object.bound_y = 2
	object.bound_width = 32
	object.bound_height = 32

	var/pixloc/location = pixloc(1, 1, 1)
	ASSERT(location.x == 1)
	ASSERT(location.y == 1)
	ASSERT(location.z == 1)
	ASSERT(location.step_x == 0)
	ASSERT(location.step_y == 0)

	del(object)
