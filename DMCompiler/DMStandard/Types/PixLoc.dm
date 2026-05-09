/pixloc
	var/turf/loc
	var/step_x
	var/step_y
	var/x as num
	var/y as num
	var/z as num

	proc/New(pixel_x, pixel_y, pixel_z)
		src.x = pixel_x
		src.y = pixel_y
		src.z = pixel_z
		if (!isnull(pixel_x) && !isnull(pixel_y) && !isnull(pixel_z))
			var/tile_x = floor((pixel_x - 1) / world.icon_size) + 1
			var/tile_y = floor((pixel_y - 1) / world.icon_size) + 1

			src.step_x = pixel_x - ((tile_x - 1) * world.icon_size) - 1
			src.step_y = pixel_y - ((tile_y - 1) * world.icon_size) - 1
			src.loc = locate(tile_x, tile_y, pixel_z)

/proc/pixloc(x, y, z)
	return new /pixloc(x, y, z)
