/proc/RunTest()
	var/obj/O = new()

	animate(O, pixel_x = 8, time = 2, tag = "move")
	animate(O, pixel_y = 4, time = 2, tag = "move")
	animate(O, tag = "move")

	animate(O, alpha = 128, time = 1, tag = "fade", command = null)
	animate(O, tag = "fade")
