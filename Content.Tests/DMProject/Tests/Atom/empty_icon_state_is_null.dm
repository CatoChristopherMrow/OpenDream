// NOBYOND - OpenDream normalizes empty appearance icon_state values to null
/obj/empty_icon_state
	icon_state = ""

/proc/RunTest()
	var/obj/empty_icon_state/type_path = /obj/empty_icon_state
	var/obj/empty_icon_state/instance = new

	ASSERT(isnull(initial(type_path.icon_state)))
	ASSERT(isnull(instance.icon_state))

	instance.icon_state = "filled"
	ASSERT(instance.icon_state == "filled")

	instance.icon_state = ""
	ASSERT(isnull(instance.icon_state))
