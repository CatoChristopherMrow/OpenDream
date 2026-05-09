// NOBYOND - implementation smoke test is not a BYOND parity test
/proc/RunTest()
	ASSERT(isnull(missile(/obj, locate(1, 1, 1), locate(1, 1, 1))))

	var/savefile/save = new()
	ASSERT(isnull(save.ImportText("/", "value = 1\n")))

	var/vector/a = vector(1, 0)
	var/vector/b = vector(0, 1)
	var/vector/cross2d = a.Cross(b)
	ASSERT(istype(cross2d, /vector))
	ASSERT(cross2d.x == 0)
	ASSERT(cross2d.y == 0)
	ASSERT(cross2d.z == 1)

	var/vector/c = vector(1, 0, 0)
	var/vector/d = vector(0, 1, 0)
	var/vector/crossed = c.Cross(d)
	ASSERT(crossed.x == 0)
	ASSERT(crossed.y == 0)
	ASSERT(crossed.z == 1)

	var/vector/turned = a.Turn(90)
	ASSERT(abs(turned.x) < 0.0001)
	ASSERT(abs(turned.y - 1) < 0.0001)
