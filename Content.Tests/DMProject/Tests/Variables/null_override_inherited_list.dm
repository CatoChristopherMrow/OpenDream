/obj/null_override_parent
	var/list/allowed = list(/obj)

/obj/null_override_parent/child
	allowed = null

/proc/RunTest()
	var/obj/null_override_parent/child/C = new

	ASSERT(isnull(C.allowed))
	ASSERT(isnull(initial(C.allowed)))
