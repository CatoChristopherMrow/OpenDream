/proc/RunTest()
	var/database/db = new("reset.db")
	var/database/query/query = new("CREATE TABLE test (id int, name string)")
	query.Execute(db)

	query.Add("INSERT INTO test VALUES (?, ?)", 1, "one")
	query.Execute(db)
	query.Add("INSERT INTO test VALUES (?, ?)", 2, "two")
	query.Execute(db)

	query.Add("SELECT * FROM test ORDER BY id")
	query.Execute(db)
	query.NextRow()
	ASSERT(query.GetColumn(1) == 1)

	query.NextRow()
	ASSERT(query.GetColumn(1) == 2)

	query.Reset()
	query.NextRow()
	ASSERT(query.GetColumn(1) == 1)
	ASSERT(query.GetColumn(2) == "one")

	del(query)
	del(db)
	fdel("reset.db")
