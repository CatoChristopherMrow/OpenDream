/obj/modified_type_test
	var/amount = 1
	var/label = "base"

/proc/RunTest()
	var/list/choices = list(/obj/modified_type_test{amount = 30; label = "override"})
	var/path = choices[1]
	ASSERT(ispath(path, /obj/modified_type_test))
	ASSERT(path == /obj/modified_type_test)
	ASSERT(path != /obj)

	var/obj/modified_type_test/instance = new path
	ASSERT(instance.amount == 30)
	ASSERT(instance.label == "override")
