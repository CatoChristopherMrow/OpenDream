/proc/RunTest()
	var/icon/source = icon('icons.dmi', "mob")
	var/icon/holder = icon('icons.dmi', "mob")

	holder.Insert(source, "renamed")

	ASSERT("renamed" in holder.IconStates())
	ASSERT(holder.GetPixel(1, 1, "renamed") == source.GetPixel(1, 1))

	var/list/icons = list()
	icons["from_assoc_loop"] = source
	for(var/state in icons)
		holder.Insert(icons[state], state)

	ASSERT("from_assoc_loop" in holder.IconStates())
	ASSERT(holder.GetPixel(1, 1, "from_assoc_loop") == source.GetPixel(1, 1))

	holder.Insert(source, "/obj/item/screwdriver")

	ASSERT("/obj/item/screwdriver" in holder.IconStates())
	ASSERT(holder.GetPixel(1, 1, "/obj/item/screwdriver") == source.GetPixel(1, 1))

	var/atom/typepath = /obj/icon_insert_path_state
	var/list/path_icons = list()
	path_icons["[typepath]"] = source
	for (var/state in path_icons)
		holder.Insert(path_icons[state], state)

	ASSERT("/obj/icon_insert_path_state" in holder.IconStates())
	ASSERT(holder.GetPixel(1, 1, "/obj/icon_insert_path_state") == source.GetPixel(1, 1))

	ASSERT(fcopy(holder, "tmp_icon_insert_default_state.dmi"))
	var/icon/reloaded = icon("tmp_icon_insert_default_state.dmi")
	ASSERT("/obj/item/screwdriver" in reloaded.IconStates())
	ASSERT("/obj/icon_insert_path_state" in reloaded.IconStates())

/obj/icon_insert_path_state
