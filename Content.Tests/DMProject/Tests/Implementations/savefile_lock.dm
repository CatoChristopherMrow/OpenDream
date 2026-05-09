/proc/RunTest()
	fdel("lock.sav")

	var/savefile/first = new("lock.sav")
	ASSERT(first.Lock())

	var/savefile/second = new("lock.sav")
	ASSERT(second.Lock())

	first.Unlock()
	ASSERT(second.Lock())
	second.Unlock()

	del(first)
	del(second)
	fdel("lock.sav")
