#define SEND_TEST_SIGNAL(target, sigtype, arguments...) (!target._listen_lookup?[sigtype] ? 0 : target._SendSignal(sigtype, list(target, ##arguments)))

/datum/var/list/_listen_lookup
/datum/var/list/_signal_procs

/datum/proc/RegisterTestSignal(datum/target, signal_type, proctype, override = FALSE)
	var/list/procs = (_signal_procs ||= list())
	var/list/target_procs = (procs[target] ||= list())
	var/list/lookup = (target._listen_lookup ||= list())

	var/exists = target_procs[signal_type]
	target_procs[signal_type] = proctype

	if(exists && !override)
		CRASH("[signal_type] overridden")

	var/list/looked_up = lookup[signal_type]
	if(isnull(looked_up))
		lookup[signal_type] = src
	else if(!islist(looked_up))
		lookup[signal_type] = list(looked_up, src)
	else
		looked_up += src

/datum/proc/UnregisterTestSignal(datum/target, sig_type_or_types)
	var/list/lookup = target._listen_lookup
	if(!_signal_procs || !_signal_procs[target] || !lookup)
		return
	if(!islist(sig_type_or_types))
		sig_type_or_types = list(sig_type_or_types)
	for(var/sig in sig_type_or_types)
		if(!_signal_procs[target][sig])
			continue
		switch(length(lookup[sig]))
			if(2)
				lookup[sig] = (lookup[sig] - src)[1]
			if(1)
				if(src in lookup[sig])
					lookup -= sig
					if(!length(lookup))
						target._listen_lookup = null
						break
			if(0)
				if(lookup[sig] != src)
					continue
				lookup -= sig
				if(!length(lookup))
					target._listen_lookup = null
					break
			else
				lookup[sig] -= src

	_signal_procs[target] -= sig_type_or_types
	if(!_signal_procs[target].len)
		_signal_procs -= target

/datum/proc/_SendSignal(sigtype, list/arguments)
	var/target = _listen_lookup[sigtype]
	if(!length(target))
		var/datum/listening_datum = target
		return call(listening_datum, listening_datum._signal_procs[src][sigtype])(arglist(arguments))

	for(var/i in 1 to length(target))
		var/datum/listening_datum = target[i]
		call(listening_datum, listening_datum._signal_procs[src][sigtype])(arglist(arguments))

/datum/signal_unregister_source
	var/atom/location

/datum/signal_unregister_source/Del()
	SEND_TEST_SIGNAL(src, "qdeleting")
	return ..()

/datum/signal_unregister_element
	var/list/turf_sources = list()

/datum/signal_unregister_element/proc/Attach(datum/signal_unregister_source/source)
	RegisterTestSignal(source, "qdeleting", "OnSourceDelete", override = TRUE)
	register_turf(source, source.location)

/datum/signal_unregister_element/proc/OnSourceDelete(datum/signal_unregister_source/source)
	Detach(source)

/datum/signal_unregister_element/proc/Detach(datum/signal_unregister_source/source)
	UnregisterTestSignal(source, "qdeleting")
	unregister_turf(source, source.location)

/datum/signal_unregister_element/proc/check()
	return

/datum/signal_unregister_element/proc/pre_change()
	return

/datum/signal_unregister_element/proc/register_turf(datum/signal_unregister_source/source, atom/location)
	if(!(location in turf_sources))
		RegisterTestSignal(location, "reset", "check")
		RegisterTestSignal(location, "change", "pre_change")
	turf_sources[location] ||= list()
	turf_sources[location] |= list(ref(source))

/datum/signal_unregister_element/proc/unregister_turf(datum/signal_unregister_source/source, atom/location)
	turf_sources[location] -= ref(source)
	if(!length(turf_sources[location]))
		turf_sources -= location
		UnregisterTestSignal(location, list("reset", "change"))

/world
	maxx = 1
	maxy = 1
	maxz = 1

/proc/RunTest()
	world.maxx = 1
	world.maxy = 1
	world.maxz = 1

	var/turf/T = locate(1, 1, 1)
	var/datum/signal_unregister_element/element = new

	var/datum/signal_unregister_source/source_a = new
	source_a.location = T
	element.Attach(source_a)

	var/datum/signal_unregister_source/source_b = new
	source_b.location = T
	element.Attach(source_b)

	del(source_a)

	var/datum/signal_unregister_source/source_c = new
	source_c.location = T
	element.Attach(source_c)

	del(source_b)
	del(source_c)

	ASSERT(!element._signal_procs || !element._signal_procs[T])
	ASSERT(!T._listen_lookup)
