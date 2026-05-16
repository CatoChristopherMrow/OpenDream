/proc/RunTest()
	var/list/default_states = icon_states('turf.dmi')
	ASSERT(length(default_states))
	ASSERT("" in default_states)

	ASSERT(length(icon_states('turf.dmi', 1)) == length(default_states))
	ASSERT(length(icon_states('turf.dmi', 2)) == length(default_states))
