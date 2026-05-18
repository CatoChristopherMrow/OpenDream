/obj/scope_initial_list_source
	var/list/materials = list(/datum = 35)

/obj/scope_initial_list_copy
	var/list/materials = /obj/scope_initial_list_source::materials

/proc/RunTest()
	var/obj/scope_initial_list_copy/copy = new
	ASSERT(copy.materials)
	ASSERT(copy.materials[/datum] == 35)
