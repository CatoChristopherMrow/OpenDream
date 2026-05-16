/proc/RunTest()
	var/atom/A = /atom
	var/obj/O = /obj

	ASSERT(isnull(initial(A.icon_state)))
	ASSERT(isnull(initial(O.icon_state)))
