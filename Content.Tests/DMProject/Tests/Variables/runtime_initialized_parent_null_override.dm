/var/runtime_initialized_parent_path = /obj/runtime_initialized_parent_item

/obj/runtime_initialized_parent_item

/obj/runtime_initialized_parent
	var/list/allowed = list(runtime_initialized_parent_path)

/obj/runtime_initialized_parent/child
	allowed = null

/proc/RunTest()
	var/obj/runtime_initialized_parent/child/C = new

	ASSERT(isnull(C.allowed))
	ASSERT(isnull(initial(C.allowed)))
