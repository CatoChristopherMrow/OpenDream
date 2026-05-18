/proc/RunTest()
	var/atom/movable/holder = new()
	var/atom/movable/victim = new()
	var/baseline = refcount(victim)

	holder.vis_contents += victim
	ASSERT(refcount(victim) == baseline + 1)

	holder.vis_contents.Cut()
	ASSERT(refcount(victim) == baseline)
