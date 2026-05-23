/proc/RunTest()
	var/list/decoded = json_decode(@'["alpha","beta"]')

	ASSERT(decoded[1] == "alpha")
	ASSERT(decoded[2] == "beta")
	ASSERT(isnull(decoded["alpha"]))
	ASSERT(isnull(decoded["beta"]))
