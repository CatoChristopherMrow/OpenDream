/proc/RunTest()
	var/obj/object = new
	object.suffix = "suffix"
	object.vis_locs = list()
	object.infra_luminosity = 2
	object.luminosity = 3
	object.step_x = 4
	object.step_y = 5
	object.vis_flags = 6

	ASSERT(object.suffix == "suffix")
	ASSERT(islist(object.vis_locs))
	ASSERT(object.infra_luminosity == 2)
	ASSERT(object.luminosity == 3)
	ASSERT(object.step_x == 4)
	ASSERT(object.step_y == 5)
	ASSERT(object.vis_flags == 6)

	object.MouseDown(null, "map", "left=1")
	object.MouseDrag(null, null, null, null, null, null)
	object.MouseMove(null, null, null)
	object.MouseUp(null, null, null)
	object.MouseWheel(0, 1, null, null, null)

	var/atom/movable/movable = new
	movable.animate_movement = NO_STEPS
	movable.step_size = 12
	movable.bound_x = 1
	movable.bound_y = 2
	movable.bound_width = 16
	movable.bound_height = 24

	ASSERT(movable.animate_movement == NO_STEPS)
	ASSERT(islist(movable.locs))
	ASSERT(movable.step_size == 12)
	ASSERT(movable.bound_x == 1)
	ASSERT(movable.bound_y == 2)
	ASSERT(movable.bound_width == 16)
	ASSERT(movable.bound_height == 24)
	ASSERT(movable.bounds == "2,3 to 17,26")

	movable.bounds = "1,2 to 16,24"
	ASSERT(movable.bound_x == 0)
	ASSERT(movable.bound_y == 1)
	ASSERT(movable.bound_width == 16)
	ASSERT(movable.bound_height == 23)
	ASSERT(movable.bounds == "1,2 to 16,24")

	var/mob/mob = new
	mob.see_infrared = 1
	mob.see_in_dark = 7

	ASSERT(islist(mob.group))
	var/group_write_error = FALSE
	try
		mob.group = list("alpha")
	catch
		group_write_error = TRUE
	ASSERT(group_write_error)
	ASSERT(mob.see_infrared == 1)
	ASSERT(mob.see_in_dark == 7)

	del(object)
	del(movable)
	del(mob)
