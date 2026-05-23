/obj/static_var_test
	var/static_value = "base"

/obj/static_var_test/child
	static_value = "child"

/proc/RunTest()
	var/obj/static_var_test/T = /obj/static_var_test/child

	ASSERT(T::static_value == "child")
