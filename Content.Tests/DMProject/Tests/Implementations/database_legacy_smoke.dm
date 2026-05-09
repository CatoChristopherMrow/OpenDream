// NOBYOND - legacy _dm_db shim smoke test is OpenDream-specific
/proc/RunTest()
	fdel("legacy_parity.db")

	var/database/db = _dm_db_new_con("legacy_parity.db")
	ASSERT(istype(db, /database))
	ASSERT(_dm_db_is_connected(db))

	var/database/query/create = _dm_db_new_query("CREATE TABLE test (id int, name string)")
	ASSERT(_dm_db_execute(create, db, null, null, null))

	ASSERT(_dm_db_execute(_dm_db_new_query("INSERT INTO test VALUES (?, ?)", 2, "two"), db, null, null, null))
	ASSERT(_dm_db_execute(_dm_db_new_query("INSERT INTO test VALUES (?, ?)", 1, "one"), db, null, null, null))

	var/database/query/select = _dm_db_new_query("SELECT id, name FROM test ORDER BY id")
	ASSERT(_dm_db_execute(select, db, null, null, null))

	ASSERT(_dm_db_next_row(select, null, null))
	ASSERT(select.GetColumn("id") == 1)
	ASSERT(select.GetColumn("name") == "one")

	ASSERT(_dm_db_next_row(select, null, null))
	ASSERT(select.GetColumn(1) == 2)
	ASSERT(select.GetColumn(2) == "two")
	ASSERT(!_dm_db_next_row(select, null, null))

	var/list/columns = _dm_db_columns(select, null)
	ASSERT(columns.len == 2)
	ASSERT(columns[1] == "id")
	ASSERT(columns[2] == "name")
	ASSERT(_dm_db_columns(select, 2) == "name")

	var/database/query/delete_query = _dm_db_new_query("DELETE FROM test WHERE id = ?", 2)
	ASSERT(_dm_db_execute(delete_query, db, null, null, null))
	ASSERT(isnum(_dm_db_rows_affected(delete_query)))

	ASSERT(_dm_db_quote(null, null) == "NULL")
	ASSERT(_dm_db_quote("can't", null) == "'can''t'")
	ASSERT(isnull(_dm_db_error_msg(db)))
	ASSERT(_dm_db_close(db))
	ASSERT(!_dm_db_is_connected(db))

	fdel("legacy_parity.db")
