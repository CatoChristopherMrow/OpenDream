/obj/istype_dynamic_parent

/obj/istype_dynamic_parent/child

/proc/RunTest()
	var/obj/O = new /obj/istype_dynamic_parent/child
	var/list/allowed = list(/obj/istype_dynamic_parent)
	var/type = allowed[1]

	ASSERT(istype(O, type))
