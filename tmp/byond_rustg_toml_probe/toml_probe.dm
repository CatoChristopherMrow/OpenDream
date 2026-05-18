#define RUST_G "/mnt/c/Dev/Cloned repos/SPLURT/S.P.L.U.R.T-tg/librust_g64.so"

/world/New()
	. = ..()
	var/path = "/mnt/c/Dev/Cloned repos/SPLURT/S.P.L.U.R.T-tg/_maps/bubber/automapper/automapper_config.toml"
	var/raw = call(RUST_G, "toml_file_to_json")(path)
	world.log << "raw_prefix=[copytext(raw, 1, 200)]"
	var/list/output = json_decode(raw || "null")
	world.log << "success=[output["success"]]"
	var/list/content = json_decode(output["content"])
	var/list/map_files = content["templates"]["birdshot_armory"]["map_files"]
	world.log << "map_files_len=[length(map_files)]"
	for(var/map in map_files)
		world.log << "iter=[map] by_value=[isnull(map_files[map]) ? "null" : map_files[map]] by_index=[map_files[1]]"
	del(world)
