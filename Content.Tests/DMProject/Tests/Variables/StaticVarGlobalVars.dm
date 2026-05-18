/obj/static_var_holder
	var/static/obj/static_target

/proc/RunTest()
	var/obj/victim = new()
	var/obj/static_var_holder/holder = new()

	var/baseline = refcount(victim)
	holder.static_target = victim
	ASSERT(refcount(victim) == baseline + 1)

	var/list/copied_globals = list()
	for (var/key in global.vars)
		copied_globals[key] = global.vars[key]

	ASSERT(copied_globals["static_target"] == victim)
	ASSERT(refcount(victim) == baseline + 2)

	holder.static_target = null
	ASSERT(refcount(victim) == baseline + 1)

	copied_globals.Cut()
	ASSERT(refcount(victim) == baseline)
