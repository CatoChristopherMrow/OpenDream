/obj/ispath_dynamic_parent

/obj/ispath_dynamic_parent/child

/proc/RunTest()
	var/list/allowed = list(/obj/ispath_dynamic_parent)
	var/type = allowed[1]

	ASSERT(ispath(/obj/ispath_dynamic_parent/child, type))
