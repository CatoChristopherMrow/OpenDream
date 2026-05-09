/proc/RunTest()
	var/obj/O = new()
	var/matrix/M = matrix(1, 2, 3, 4, 5, 6)
	var/sound/S = sound(null, 1, 1, 2, 75)

	ASSERT(isnull(S.file))
	ASSERT(S.repeat == 1)
	ASSERT(S.wait == 1)
	ASSERT(S.channel == 2)
	ASSERT(S.volume == 75)

	S.atom = O
	S.transform = M
	S.pitch = 1.5
	S.pan = -20
	S.params = list("flag" = "value")
	S.falloff = 2
	S.x = 3
	S.y = 4
	S.z = 5
	S.environment = 6
	S.echo = 7
	S.len = 8
	S.priority = 9
	S.status = SOUND_PAUSED

	ASSERT(S.atom == O)
	ASSERT(S.transform == M)
	ASSERT(S.pitch == 1.5)
	ASSERT(S.pan == -20)
	ASSERT(S.params["flag"] == "value")
	ASSERT(S.falloff == 2)
	ASSERT(S.x == 3)
	ASSERT(S.y == 4)
	ASSERT(S.z == 5)
	ASSERT(S.environment == 6)
	ASSERT(S.echo == 7)
	ASSERT(S.len == 8)
	ASSERT(S.priority == 9)
	ASSERT(S.status == SOUND_PAUSED)

	O.icon_w = 16
	O.icon_z = 2
	ASSERT(O.icon_w == 16)
	ASSERT(O.icon_z == 2)

	del(O)
