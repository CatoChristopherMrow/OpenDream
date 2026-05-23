/proc/RunTest()
	var/list/assoc = list("datum" = /datum)
	var/key_seen
	var/value_seen

	for (var/key, value in assoc)
		key_seen = key
		value_seen = value

	ASSERT(key_seen == "datum")
	ASSERT(value_seen == /datum)
