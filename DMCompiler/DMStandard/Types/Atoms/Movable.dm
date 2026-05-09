/atom/movable
	var/screen_loc

	var/animate_movement = FORWARD_STEPS
	var/list/locs = null
	var/glide_size = 0
	var/step_size
	var/tmp/bound_x
	var/tmp/bound_y
	var/tmp/bound_width
	var/tmp/bound_height

	//Undocumented var. "[x],[y]" or "[x],[y] to [x2],[y2]" based on bound_* vars
	var/bounds

	var/particles/particles 

	proc/Bump(atom/Obstacle)

	proc/Move(atom/NewLoc, Dir=0) as num
		if (isnull(NewLoc) || loc == NewLoc)
			return FALSE

		if (Dir != 0)
			dir = Dir

		if(!isnull(loc))
			if (!loc.Exit(src, NewLoc))
				return FALSE
			// Ensure the atoms on the turf also permit this exit
			for (var/atom/movable/exiting in loc)
				if (!exiting.Uncross(src))
					return FALSE

		if (NewLoc.Enter(src, loc))
			var/atom/oldloc = loc
			var/area/oldarea = oldloc?.loc
			var/area/newarea = NewLoc.loc
			loc = NewLoc

			// First, call Exited() on the old area
			if (newarea != oldarea)
				oldarea?.Exited(src, loc)

			// Second, call Exited() on the old turf and Uncrossed() on its contents
			oldloc?.Exited(src, loc)
			for (var/atom/movable/uncrossed in oldloc)
				uncrossed.Uncrossed(src)

			// Third, call Entered() on the new turf and Crossed() on its contents
			loc.Entered(src, oldloc)
			for (var/atom/movable/crossed in loc)
				crossed.Crossed(src)

			// Fourth, call Entered() on the new area
			if (newarea != oldarea)
				newarea.Entered(src, oldloc)

			return TRUE
		else
			return FALSE
