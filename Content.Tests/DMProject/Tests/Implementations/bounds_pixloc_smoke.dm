/world
	maxx = 3
	maxy = 3
	maxz = 1

/proc/RunTest()
	world.maxx = 3
	world.maxy = 3
	world.maxz = 1

	var/pixloc/base = pixloc(1, 1, 1)
	ASSERT(base.x == 1)
	ASSERT(base.y == 1)
	ASSERT(base.z == 1)
	ASSERT(base.step_x == 0)
	ASSERT(base.step_y == 0)

	var/pixloc/tile_edge = pixloc(32, 32, 1)
	ASSERT(tile_edge.step_x == 31)
	ASSERT(tile_edge.step_y == 31)

	var/pixloc/next_tile = pixloc(33, 65, 1)
	ASSERT(next_tile.step_x == 0)
	ASSERT(next_tile.step_y == 0)

	var/turf/center = locate(1, 1, 1)
	var/turf/offset_turf = locate(2, 3, 1)
	var/pixloc/turf_pixloc = offset_turf.pixloc
	ASSERT(turf_pixloc.x == 33)
	ASSERT(turf_pixloc.y == 65)
	ASSERT(turf_pixloc.z == 1)
	ASSERT(turf_pixloc.loc == offset_turf)
	ASSERT(turf_pixloc.step_x == 0)
	ASSERT(turf_pixloc.step_y == 0)

	var/obj/O = new(center)
	O.step_x = 2
	O.step_y = 3
	O.bound_x = 1
	O.bound_y = 2
	O.bound_width = 32
	O.bound_height = 32

	var/list/in_bounds = bounds(O, 0)
	ASSERT(O in in_bounds)

	var/list/out_bounds = obounds(O, 0)
	ASSERT(!(O in out_bounds))

	var/pixloc/object_base = pixloc(O.x, O.y, O.z)
	ASSERT(object_base.loc == center)
	ASSERT(object_base.step_x == 0)
	ASSERT(object_base.step_y == 0)

	var/pixloc/object_pixloc = O.pixloc
	ASSERT(object_pixloc.x == 4)
	ASSERT(object_pixloc.y == 6)
	ASSERT(object_pixloc.z == 1)
	ASSERT(object_pixloc.loc == center)
	ASSERT(object_pixloc.step_x == 3)
	ASSERT(object_pixloc.step_y == 5)

	var/pixloc/east = bound_pixloc(O, EAST)
	ASSERT(east.x == 36)
	ASSERT(east.y == 22)
	ASSERT(east.z == O.z)
	ASSERT(east.loc == center)
	ASSERT(east.step_x == 35)
	ASSERT(east.step_y == 21)

	var/pixloc/west = bound_pixloc(O, WEST)
	ASSERT(west.x == 4)
	ASSERT(west.y == 22)
	ASSERT(west.step_x == 3)
	ASSERT(west.step_y == 21)

	var/pixloc/north = bound_pixloc(O, NORTH)
	ASSERT(north.x == 20)
	ASSERT(north.y == 38)
	ASSERT(north.loc == center)
	ASSERT(north.step_x == 19)
	ASSERT(north.step_y == 37)

	var/pixloc/south = bound_pixloc(O, SOUTH)
	ASSERT(south.x == 20)
	ASSERT(south.y == 6)
	ASSERT(south.step_x == 19)
	ASSERT(south.step_y == 5)

	var/pixloc/northeast = bound_pixloc(O, NORTH|EAST)
	ASSERT(northeast.step_x == east.step_x)
	ASSERT(northeast.step_y == north.step_y)

	var/obj/P = new(center)
	P.pixloc = pixloc(40, 70, 1)
	ASSERT(P.loc == offset_turf)
	ASSERT(P.x == 2)
	ASSERT(P.y == 3)
	ASSERT(P.z == 1)
	ASSERT(P.step_x == 7)
	ASSERT(P.step_y == 5)
	P.pixloc = null
	ASSERT(isnull(P.loc))
	ASSERT(isnull(P.pixloc))

	ASSERT(isnull(bound_pixloc(null, NORTH)))
	del(P)
	del(O)
