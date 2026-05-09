/proc/RunTest()
	var/obj/container = new
	var/obj/container_item = new(container)

	ASSERT(container_item in container)
	ASSERT(container_item in container.contents)
	ASSERT(container.contents.Find(container_item) != 0)

	ASSERT(container in world)
	ASSERT(container_item in world)
