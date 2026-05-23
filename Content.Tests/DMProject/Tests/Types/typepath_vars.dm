//# issue 2194

/datum/typepath_vars_parent

/datum/typepath_vars_parent/child
	var/static/test_value = "initial child value"

/proc/RunTest()
	var/datum/typepath_vars_parent/child/path_value = /datum/typepath_vars_parent/child

	ASSERT(path_value.type == /datum/typepath_vars_parent/child)
	ASSERT(path_value.parent_type == /datum/typepath_vars_parent)
	ASSERT(path_value.test_value == "initial child value")
	ASSERT(new path_value.type)
