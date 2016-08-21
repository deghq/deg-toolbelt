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
	public interface ITableRepository
	{
		List<Table> FindTables(params string[] names);
		List<Column> FindTableColumns(string tableName);
//		List<Table> FindAllTables();
	}
	
	public class TableService
	{
		ITableRepository r;
		
		public TableService(ITableRepository r)
		{
			this.r = r;
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
		
		public Repository GetBaseRepository(string @namespace)
		{
			if (r is MySqlTableRepository) {
				return new BaseMySqlRepository(@namespace);
			} else {
				return new BaseSqlRepository(@namespace);
			}
		}
		
		public Repository GetRepository(Table t, string @namespace)
		{
			if (r is MySqlTableRepository) {
				return new MySqlRepository(t, @namespace);
			} else {
				return new SqlRepository(t, @namespace);
			}
		}
	}
}
