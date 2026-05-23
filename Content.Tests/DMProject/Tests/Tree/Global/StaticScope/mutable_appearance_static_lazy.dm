// NOBYOND - OpenDream static initialization laziness regression test
var/static/mutable_appearance_static_initialized = FALSE

/proc/make_static_mutable_appearance()
	mutable_appearance_static_initialized = TRUE
	return new /mutable_appearance()

/datum/static_mutable_appearance_holder
	var/static/mutable_appearance/static_appearance = make_static_mutable_appearance()

/proc/RunTest()
	ASSERT(!mutable_appearance_static_initialized)

	var/datum/static_mutable_appearance_holder/holder = new
	ASSERT(holder.static_appearance)
	ASSERT(mutable_appearance_static_initialized)
