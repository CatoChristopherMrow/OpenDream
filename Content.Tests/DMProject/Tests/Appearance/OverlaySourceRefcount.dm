/proc/RunTest()
	world.maxx = 1
	world.maxy = 1
	world.maxz = 1

	var/obj/owner = new(locate(1, 1, 1))
	var/obj/source = new(locate(1, 1, 1))
	var/baseline = refcount(source)

	owner.overlays += source
	var/after_add = refcount(source)
	if (after_add != baseline + 1)
		CRASH("Expected overlays += source to hold one source ref; baseline [baseline], got [after_add]")

	owner.overlays.Cut()
	ASSERT(refcount(source) == baseline)

	owner.overlays = list(source)
	var/after_replace = refcount(source)
	if (after_replace != baseline + 1)
		CRASH("Expected overlays = list(source) to hold one source ref; baseline [baseline], got [after_replace]")

	owner.overlays -= source
	ASSERT(refcount(source) == baseline)
