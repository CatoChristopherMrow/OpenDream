/obj/initial_path_var
	icon_state = "parent"

/obj/initial_path_var/child
	icon_state = null

/obj/initial_path_var/child/grandchild
	icon_state = "grandchild"

/proc/RunTest()
	var/atom/item = /obj/initial_path_var/child/grandchild
	ASSERT(initial(item.icon_state) == "grandchild")
