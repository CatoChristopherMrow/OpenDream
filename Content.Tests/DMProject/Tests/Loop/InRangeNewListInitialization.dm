// NOBYOND - relies on OpenDream accepting direct Initialize() calls with arguments
/datum/gas_holder
	var/volume = 0

/obj/machinery/test_component
	var/device_type = 0
	var/list/datum/gas_holder/airs
	var/list/parents
	var/list/nodes

/obj/machinery/test_component/Initialize()
	nodes = new(device_type)
	var/list/node_connects[device_type]

	for(var/i in 1 to device_type)
		node_connects[i] = i

	ASSERT(node_connects.len == device_type)
	ASSERT(node_connects[1] == 1)

	nodes[1] = src
	airs = new(device_type)
	parents = new(device_type)

	for(var/i in 1 to device_type)
		if(airs[i])
			continue

		var/datum/gas_holder/component_mixture = new
		component_mixture.volume = 200
		airs[i] = component_mixture

	return ..()

/obj/machinery/test_component/proc/set_pipenet(datum/pipeline/reference, obj/machinery/test_component/target_component)
	parents[nodes.Find(target_component)] = reference

/obj/machinery/test_component/proc/return_pipenet_airs(datum/pipeline/reference)
	var/list/returned_air = list()

	for(var/i in 1 to parents.len)
		if(parents[i] == reference)
			returned_air += airs[i]

	return returned_air

/obj/machinery/test_component/unary
	device_type = 1

/obj/machinery/test_component/unary/layer4

/datum/pipeline

/proc/RunTest()
	var/obj/machinery/test_component/unary/layer4/component = new
	component.Initialize(TRUE)

	ASSERT(component.device_type == 1)
	ASSERT(component.airs.len == 1)
	ASSERT(istype(component.airs[1], /datum/gas_holder))
	ASSERT(component.airs[1].volume == 200)

	var/datum/pipeline/pipeline = new
	component.set_pipenet(pipeline, component)

	var/list/airs = component.return_pipenet_airs(pipeline)
	ASSERT(airs.len == 1)
	ASSERT(airs[1] == component.airs[1])
