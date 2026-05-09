// NOBYOND - implementation smoke test is not a BYOND parity test
/proc/RunTest()
	var/sound/S = sound()
	var/obj/O = new()
	var/matrix/M = matrix()

	S.atom = O
	S.transform = M
	ASSERT(S.atom == O)
	ASSERT(S.transform == M)

	O.icon_w = 2
	O.icon_z = 3
	ASSERT(O.icon_w == 2)
	ASSERT(O.icon_z == 3)
	ASSERT(isnull(O.pixloc))

	O.pixloc = pixloc(1, 1, 1)
	ASSERT(isnull(O.pixloc))

/client/proc/TestImplementationVars()
	lazy_eye = lazy_eye
	edge_limit = edge_limit
	pixel_x = pixel_x
	pixel_y = pixel_y
	pixel_z = pixel_z
	pixel_w = pixel_w
	inactivity = inactivity
	tick_lag = tick_lag
	script = script
	color = color
	control_freak = control_freak
	fps = fps
	dir = dir
	glide_size = glide_size
	virtual_eye = virtual_eye
	bounds = bounds
	bound_x = bound_x
	bound_y = bound_y
	bound_width = bound_width
	bound_height = bound_height
