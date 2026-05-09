/proc/RunTest()
	var/icon/base = new('icons.dmi', "mob")
	var/icon/overlay = new('icons.dmi', "mob")

	base.Blend(overlay, ICON_OVERLAY)
	base.Shift(WEST, 8)
	base.Crop(1, 1, 24, 32)

	ASSERT(base.Width() == 24)
	ASSERT(base.Height() == 32)

