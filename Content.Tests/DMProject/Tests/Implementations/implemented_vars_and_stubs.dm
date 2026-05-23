// NOBYOND - implementation smoke test is not a BYOND parity test
/proc/RunTest()
	ASSERT(world.OpenPort(0))
	ASSERT(!world.OpenPort(-1))

	var/list/profile = world.Profile(0)
	ASSERT(profile.len)
	var/profile_json = world.Profile(0, null, "json")
	ASSERT(profile_json == "\[\]")

	ASSERT(isnum(world.cpu))
	ASSERT(world.map_cpu == 0)
	world.status = "testing"
	world.visibility = 1
	ASSERT(world.status == "testing")
	ASSERT(world.visibility == 1)
	ASSERT(istext(world.internet_address))

	var/sound/sound = new
	sound.repeat = 2
	sound.pitch = 1.25
	sound.pan = -10
	sound.params = "abc"
	sound.falloff = 2
	sound.x = 1
	sound.y = 2
	sound.z = 3
	sound.environment = 4
	sound.echo = 5
	sound.len = 6
	sound.priority = 7
	sound.status = 8

	ASSERT(sound.repeat == 2)
	ASSERT(sound.pitch == 1.25)
	ASSERT(sound.pan == -10)
	ASSERT(sound.params == "abc")
	ASSERT(sound.falloff == 2)
	ASSERT(sound.x == 1)
	ASSERT(sound.y == 2)
	ASSERT(sound.z == 3)
	ASSERT(sound.environment == 4)
	ASSERT(sound.echo == 5)
	ASSERT(sound.len == 6)
	ASSERT(sound.priority == 7)
	ASSERT(sound.status == 8)

	var/database/db = new("binobj.db")
	db._binobj = "db"
	ASSERT(db._binobj == "db")

	var/database/query/query = new("CREATE TABLE test (id int)")
	query._binobj = "query"
	ASSERT(query._binobj == "query")

	var/generator/generator = generator("num", 1, 2)
	generator._binobj = "generator"
	ASSERT(generator._binobj == "generator")

	del(query)
	del(db)
	fdel("binobj.db")
