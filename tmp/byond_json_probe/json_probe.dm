/world/New()
	. = ..()
	var/list/L = json_decode(@'["birdshot_armory.dmm"]')
	var/iterated
	for(var/map in L)
		iterated = map
		world.log << "iterated=[isnull(iterated) ? "null" : iterated]"
		world.log << "by_index=[isnull(L[1]) ? "null" : L[1]]"
		world.log << "by_value=[isnull(L[map]) ? "null" : L[map]]"
		world.log << "contains=[map in L]"
	del(world)
