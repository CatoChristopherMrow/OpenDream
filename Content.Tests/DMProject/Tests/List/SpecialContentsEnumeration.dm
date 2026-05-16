/proc/RunTest()
	var/obj/container = new
	var/obj/container_item = new(container)

	var/count = 0
	for(var/obj/item in container)
		ASSERT(item == container_item)
		count++
	ASSERT(count == 1)

	count = 0
	for(var/obj/item in container.contents)
		ASSERT(item == container_item)
		count++
	ASSERT(count == 1)
