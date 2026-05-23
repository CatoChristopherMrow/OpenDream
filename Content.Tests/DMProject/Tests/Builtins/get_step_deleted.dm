/proc/RunTest()
	var/obj/item = new /obj()
	del(item)

	ASSERT(isnull(get_step(item, NORTH)))
	ASSERT(isnull(get_step(item, 0)))
