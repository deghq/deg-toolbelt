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
	public class SqlTableRepository : ITableRepository
	{
		SqlConnection con;
		
		public SqlTableRepository(string database)
		{
			if (database != "") {
				con = new SqlConnection(string.Format("Server=localhost;Database={0};Trusted_Connection=True;", database));
			} else {
				con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["database"]].ConnectionString);
			}
		}
		
		SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
		{
			var cmd = new SqlCommand(query, con);
			OpenConnection();
			cmd.Parameters.AddRange(parameters);
			return cmd.ExecuteReader();
		}
		
		void OpenConnection()
		{
			if (con.State == ConnectionState.Closed) {
				con.Open();
			}
		}
		
		void CloseConnection()
		{
			if (con.State == ConnectionState.Open) {
				con.Close();
			}
		}
		
		public Table ReadTable(string name)
		{
			string query = @"
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = @TableName";
			Table t = null;
			using (var rs = ExecuteReader(query, new SqlParameter("@TableName", name))) {
				if (rs.Read()) {
					t = new Table {
						Database = con.Database,
						Name = rs.GetString(0)
					};
				}
			}
			CloseConnection();
			return t;
		}
		
		public List<Table> FindTables(params string[] names)
		{
			string parameterNames = "";
			int i = 1;
			foreach (var n in names) {
				parameterNames += "'" + n + "'";
				parameterNames += i++ < names.Length ? ", " : "";
			}
			string query = string.Format(
				@"
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
{0}",
				names.Length == 0 ? "" : string.Format("WHERE TABLE_NAME IN ({0})", parameterNames)
			);
			var tables = new List<Table>();
			using (var rs = ExecuteReader(query)) {
				while (rs.Read()) {
					tables.Add(
						new Table {
							Database = con.Database,
							Name = rs.GetString(0)
						}
					);
				}
			}
			CloseConnection();
			return tables;
		}
		
		public List<Column> FindTableColumns(string name)
		{
			string query = @"
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = @TableName";
			var c = new List<Column>();
			using (var rs = ExecuteReader(query, new SqlParameter("@TableName", name))) {
				while (rs.Read()) {
					c.Add(
						new Column {
							Name = rs.GetString(0),
							Type = rs.GetString(1)
						}
					);
				}
			}
			CloseConnection();
			return c;
		}
	}
}
