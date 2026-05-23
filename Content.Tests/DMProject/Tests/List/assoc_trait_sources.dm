/proc/RunTest()
	var/list/status_traits = list()
	var/trait = "example_trait"
	var/source_a = "source_a"
	var/source_b = "source_b"

	status_traits[trait] = list(source_a)

	ASSERT(status_traits[trait])
	ASSERT(source_a in status_traits[trait])
	ASSERT(!(source_b in status_traits[trait]))

	status_traits[trait] |= list(source_b)

	ASSERT(source_a in status_traits[trait])
	ASSERT(source_b in status_traits[trait])

	status_traits[trait] -= source_a

	ASSERT(!(source_a in status_traits[trait]))
	ASSERT(source_b in status_traits[trait])

	status_traits -= trait

	ASSERT(!status_traits[trait])
