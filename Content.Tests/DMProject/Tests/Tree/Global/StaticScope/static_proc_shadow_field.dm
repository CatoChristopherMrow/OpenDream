/datum/static_proc_shadow_field
	var/list/things = list("instance")

/datum/static_proc_shadow_field/proc/get_cached_things()
	var/static/list/things
	if(!things)
		things = src.things.Copy()
	return things

/proc/RunTest()
	var/datum/static_proc_shadow_field/first = new
	var/datum/static_proc_shadow_field/second = new

	var/list/first_cached = first.get_cached_things()
	var/list/second_cached = second.get_cached_things()

	ASSERT(first_cached == second_cached)
	ASSERT(first_cached != first.things)
	ASSERT(second_cached != second.things)
