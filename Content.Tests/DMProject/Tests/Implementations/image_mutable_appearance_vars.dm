/proc/RunTest()
	var/image/image = new
	image.density = 1
	image.gender = FEMALE
	image.glide_size = 4
	image.infra_luminosity = 5
	image.invisibility = 6
	image.luminosity = 7
	image.opacity = 1
	image.pixel_step_size = 8
	image.suffix = "suffix"
	image.text = "text"
	image.vis_contents = list(image)

	ASSERT(islist(image.contents))
	ASSERT(image.density == 1)
	ASSERT(image.gender == FEMALE)
	ASSERT(image.glide_size == 8)
	ASSERT(image.infra_luminosity == 5)
	ASSERT(image.invisibility == 6)
	ASSERT(image.luminosity == 7)
	ASSERT(image.opacity == 1)
	ASSERT(image.pixel_step_size == 8)
	ASSERT(image.suffix == "suffix")
	ASSERT(image.text == "text")
	var/verbs_write_error = FALSE
	try
		image.verbs = list()
	catch
		verbs_write_error = TRUE
	ASSERT(verbs_write_error)
	ASSERT(image.vis_contents.len == 0)

	var/contents_write_error = FALSE
	try
		image.contents = list("inside")
	catch
		contents_write_error = TRUE
	ASSERT(contents_write_error)

	var/mutable_appearance/appearance = new
	appearance.animate_movement = NO_STEPS
	appearance.screen_loc = "1,2"

	ASSERT(appearance.animate_movement == NO_STEPS)
	ASSERT(appearance.screen_loc == "1,2")
