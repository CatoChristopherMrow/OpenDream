/proc/RunTest()
	ASSERT(isnull(animate(null)))
	ASSERT(isnull(animate(null, alpha = 128, time = 1)))
	ASSERT(isnull(animate(alpha = 128, time = 1)))

	var/obj/O = new()
	animate(O, pixel_x = 8, time = 1)
	animate(pixel_y = 4, time = 1)
