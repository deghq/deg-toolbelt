// <file>
//  <license></license>
//  <owner name="Ian Escarro" email="ian.escarro@gmail.com"/>
// </file>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Deg.Toolbelt
{
	public class TableService
	{
		TableRepository r;
		
		public TableService(string database)
		{
			this.r = new TableRepository(database);
		}
		
		public List<Table> FindTables(params string[] names)
		{
			var tables = r.FindTables(names);
			AddColumns(tables);
			return tables;
		}
		
		void AddColumns(List<Table> tables)
		{
			foreach (var t in tables) {
				t.Columns = r.FindTableColumns(t.Name);
			}
		}
		
		public List<Table> FindAllTables()
		{
			var tables = r.FindAllTables();
			AddColumns(tables);
			return tables;
		}
	}
}
