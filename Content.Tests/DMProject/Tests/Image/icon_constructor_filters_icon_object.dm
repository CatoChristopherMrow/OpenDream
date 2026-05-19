// NOBYOND - OpenDream icon object filtering regression test
/proc/RunTest()
	var/icon/source = icon('icons.dmi', "mob")
	source.Insert('icons.dmi', "other")

	var/icon/filtered = icon(source, "mob")
	var/list/states = icon_states(filtered)

	ASSERT(length(states) == 1)
	ASSERT(states[1] == "")
