/var/list/global_allowed = list(/obj/null_override_initialize_item)

/obj/null_override_initialize_item

/obj/null_override_initialize_suit
	var/list/allowed = list(/obj)

/obj/null_override_initialize_suit/armor
	allowed = null

/obj/null_override_initialize_suit/armor/Initialize()
	. = ..()
	if(!allowed)
		allowed = global_allowed

/proc/RunTest()
	var/obj/null_override_initialize_suit/armor/A = new
	A.Initialize()

	ASSERT(istype(A.allowed, /list))
	ASSERT(A.allowed.len == 1)
	ASSERT(A.allowed[1] == /obj/null_override_initialize_item)
