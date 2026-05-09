/datum/unit_test/savefile_lock_unlock/RunTest()
	var/save_path = "data/savefile_lock_unlock.sav"
	fdel(save_path)

	var/savefile/a = new(save_path)
	var/savefile/b = new(save_path)

	ASSERT(a.Lock())
	ASSERT(!b.Lock())
	ASSERT(!b.Unlock())
	ASSERT(a.Unlock())
	ASSERT(b.Lock())
	ASSERT(b.Unlock())

	del(a)
	del(b)
	fdel(save_path)
