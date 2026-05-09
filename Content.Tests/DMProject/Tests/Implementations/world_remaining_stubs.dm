// NOBYOND - implementation smoke test is not a BYOND parity test
/proc/RunTest()
	ASSERT(world.loop_checks == 0)
	world.loop_checks = 1
	ASSERT(world.loop_checks == 1)

	ASSERT(world.movement_mode == LEGACY_MOVEMENT_MODE)

	ASSERT(istext(world.url))
	ASSERT(findtext(world.url, ":"))

	ASSERT(isnull(world.reachable))

	ASSERT(world.map_format == TOPDOWN_MAP)

	ASSERT(!world.IsBanned("key", "127.0.0.1", "computer", null))
	ASSERT(isnull(world.Repop()))
	ASSERT(isnull(world.Import()))
	ASSERT(isnull(world.Tick()))
