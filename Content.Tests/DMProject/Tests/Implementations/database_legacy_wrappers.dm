// NOBYOND - legacy _dm_db shim smoke test is OpenDream-specific
/proc/RunTest()
	var/database/db = _dm_db_new_con("legacy.db")
	ASSERT(istype(db, /database))
	ASSERT(_dm_db_is_connected(db))

	var/database/query/query = _dm_db_new_query("CREATE TABLE test (id int, name string)")
	ASSERT(istype(query, /database/query))
	ASSERT(_dm_db_execute(query, db, null, null, null))

	query = _dm_db_new_query("INSERT INTO test VALUES (?, ?)", 1, "one")
	ASSERT(_dm_db_execute(query, db, null, null, null))

	query = _dm_db_new_query("SELECT id, name FROM test ORDER BY id")
	ASSERT(_dm_db_execute(query, db, null, null, null))
	ASSERT(_dm_db_next_row(query, null, null))
	ASSERT(query.GetColumn(0) == 1)
	ASSERT(query.GetColumn(1) == "one")
	ASSERT(!_dm_db_next_row(query, null, null))

	var/list/columns = _dm_db_columns(query, null)
	ASSERT(columns.len == 2)
	ASSERT(columns[1] == "id")
	ASSERT(columns[2] == "name")
	ASSERT(_dm_db_columns(query, 1) == "id")
	ASSERT(isnum(_dm_db_rows_affected(query)))
	ASSERT(_dm_db_row_count(query) == 0)
	ASSERT(isnull(_dm_db_error_msg(query)))
	ASSERT(_dm_db_quote("Bob's", null) == "'Bob''s'")

	ASSERT(_dm_db_close(db))
	ASSERT(!_dm_db_is_connected(db))
	fdel("legacy.db")
