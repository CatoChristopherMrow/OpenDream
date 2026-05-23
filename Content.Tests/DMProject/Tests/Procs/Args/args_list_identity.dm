//# issue 2194

#define SEND_TEST_SIGNAL(target, arguments...) target.ReceiveSignal(list(target, ##arguments))

var/global/last_args_ref

/datum/args_identity_receiver/proc/ReceiveSignal(list/arguments)
	return call(src, "CheckSignal")(arglist(arguments))

/datum/args_identity_receiver/proc/CheckSignal(datum/source, list/passed_args)
	ASSERT("\ref[passed_args]" == last_args_ref)

/proc/check_direct_args_identity(a, b)
	last_args_ref = "\ref[args]"
	ASSERT("\ref[args]" == last_args_ref)

/proc/check_args_identity_through_arglist(a, b)
	last_args_ref = "\ref[args]"
	var/datum/args_identity_receiver/receiver = new
	SEND_TEST_SIGNAL(receiver, args)

/proc/RunTest()
	check_direct_args_identity("a", "b")
	check_args_identity_through_arglist("a", "b")

#undef SEND_TEST_SIGNAL
