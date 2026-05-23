//# issue 2194

/datum/uninitialized_var_holder
	var/empty

/proc/RunTest()
	var/datum/uninitialized_var_holder/holder = new
	ASSERT(isnull(holder.empty))
