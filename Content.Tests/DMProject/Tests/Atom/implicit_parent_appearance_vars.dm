/obj/implicit_parent_appearance/child
	icon_state = "child"

/proc/RunTest()
	var/obj/implicit_parent_appearance/parent_type = /obj/implicit_parent_appearance
	var/obj/implicit_parent_appearance/child/child_type = /obj/implicit_parent_appearance/child

	ASSERT(isnull(initial(parent_type.icon_state)))
	ASSERT(initial(child_type.icon_state) == "child")

	for(var/obj/obj_path as anything in typesof(/obj/implicit_parent_appearance) - /obj/implicit_parent_appearance)
		if(obj_path == /obj/implicit_parent_appearance/child)
			ASSERT(initial(obj_path.icon_state) == "child")
			continue

		ASSERT(isnull(initial(obj_path.icon_state)))
