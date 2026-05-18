/proc/RunTest()
	var/icon/generated = icon('icons.dmi', "mob")
	var/generated_resource = fcopy_rsc(generated)

	ASSERT(isicon(generated))
	ASSERT(isicon(generated_resource))
	ASSERT(isicon('icons.dmi'))
