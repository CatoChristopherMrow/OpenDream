/datum/stringify_operator_test/proc/operator""()
	return "custom-string"

/proc/RunTest()
	var/datum/stringify_operator_test/T = new
	ASSERT("[T]" == "custom-string")
