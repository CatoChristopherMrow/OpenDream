/proc/RunTest()
	var/atom/A = new /obj()
	var/mutable_appearance/underlay = new /mutable_appearance()
	underlay.plane = 1
	A.underlays += underlay

	var/mutable_appearance/copy = new(A)
	copy.plane = 2

	for (var/mutable_appearance/current as anything in copy.underlays)
		if (current.plane != copy.plane)
			copy.underlays -= current

	ASSERT(length(copy.underlays) == 0)
