// NOBYOND
/datum/qdel_marked_ref_drop
	var/gc_destroyed
	var/datum/held_ref

/datum/qdel_marked_ref_drop/Del()
	CRASH("qdel-marked object was deleted by refcount cleanup")

/obj/qdel_marked_ref_drop
	var/gc_destroyed

/obj/qdel_marked_ref_drop/Del()
	CRASH("qdel-marked movable was deleted by refcount cleanup")

/obj/qdel_marked_ref_drop/Destroy()
	loc = null
	return 0

/proc/_qdel_marked_ref_drop_queue(obj/qdel_marked_ref_drop/target)
	target.gc_destroyed = -2
	target.Destroy()
	return list(list(0, target, target.gc_destroyed))

/proc/_qdel_marked_ref_drop_check_queue(list/queue)
	var/list/queue_item = queue[1]
	var/obj/qdel_marked_ref_drop/queued_ref = queue_item[2]
	var/count = refcount(queued_ref)
	ASSERT(count == 2)
	return refcount(queued_ref)

/proc/_qdel_marked_ref_drop_handle_queue_shape(list/queue)
	var/list/queue_item = queue[1]
	var/obj/qdel_marked_ref_drop/queued_ref = queue_item[2]
	ASSERT(refcount(queued_ref) == 2)

/proc/_qdel_marked_ref_drop_create_and_queue(turf/location)
	var/obj/qdel_marked_ref_drop/created = new(location)
	var/list/queue = _qdel_marked_ref_drop_queue(created)
	created = null
	_qdel_marked_ref_drop_check_queue(queue)

/proc/RunTest()
	var/datum/held = new
	var/held_baseline = refcount(held)
	var/datum/qdel_marked_ref_drop/target = new
	ASSERT(refcount(target) == 1)
	var/target_ref = ref(target)
	target.held_ref = held
	ASSERT(refcount(held) == held_baseline + 1)
	target.gc_destroyed = -2
	target = null
	ASSERT(refcount(held) == held_baseline)
	ASSERT(isnull(locate(target_ref)))

	world.maxx = 1
	world.maxy = 1
	world.maxz = 1

	var/obj/qdel_marked_ref_drop/movable = new(locate(1, 1, 1))
	ASSERT(refcount(movable) == 2)
	var/list/queue = list(movable)
	ASSERT(refcount(movable) == 3)
	queue.Cut()
	ASSERT(refcount(movable) == 2)
	var/movable_ref = ref(movable)
	movable.gc_destroyed = -2
	movable.loc = null
	movable = null
	ASSERT(isnull(locate(movable_ref)))

	var/obj/qdel_marked_ref_drop/queued = new(locate(1, 1, 1))
	var/list/nested_queue = list(list(0, queued, 0))
	var/list/queue_item = nested_queue[1]
	var/obj/qdel_marked_ref_drop/queued_ref = queue_item[2]
	queued.gc_destroyed = -2
	queued.loc = null
	queued = null
	ASSERT(refcount(queued_ref) == 2)

	var/obj/qdel_marked_ref_drop/qdel_queued = new(locate(1, 1, 1))
	var/list/qdel_queue = _qdel_marked_ref_drop_queue(qdel_queued)
	var/list/qdel_queue_item = qdel_queue[1]
	var/obj/qdel_marked_ref_drop/qdel_queued_ref = qdel_queue_item[2]
	qdel_queued = null
	ASSERT(refcount(qdel_queued_ref) == 2)
	qdel_queued_ref = null
	_qdel_marked_ref_drop_check_queue(qdel_queue)
	_qdel_marked_ref_drop_create_and_queue(locate(1, 1, 1))
