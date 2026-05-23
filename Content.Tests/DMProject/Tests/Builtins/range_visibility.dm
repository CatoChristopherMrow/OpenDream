/obj/range_visibility_anchor

/obj/range_visibility_hidden
	invisibility = 101

/proc/RunTest()
	var/turf/T = locate(1, 1, 1)
	var/obj/range_visibility_anchor/anchor = new(T)
	var/obj/range_visibility_hidden/hidden = new(T)

	var/list/ranged = range(1, anchor)

	ASSERT(anchor in ranged)
	ASSERT(!(hidden in ranged))
