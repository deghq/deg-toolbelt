using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Deg.Toolbelt
{
	public class MySqlTableRepository : ITableRepository
	{
		MySqlConnection con;
		string database;
		
		public MySqlTableRepository(string database)
		{
			if (database != "") {
				con = new MySqlConnection(string.Format("server=localhost;user id=root;database={0}", database));
			} else {
				con = new MySqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["database"]].ConnectionString);
			}
			this.database = con.Database;
		}
		
		MySqlDataReader ExecuteReader(string query, params MySqlParameter[] parameters)
		{
			var cmd = new MySqlCommand(query, con);
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
WHERE TABLE_NAME = @TableName
AND TABLE_SCHEMA = @TableSchema
AND TABLE_TYPE = 'BASE TABLE'";
			Table t = null;
			using (var rs = ExecuteReader(query, new MySqlParameter("@TableName", name), new MySqlParameter("@TableSchema", database))) {
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
WHERE TABLE_SCHEMA = @TableSchema
AND TABLE_TYPE = 'BASE TABLE'
{0}",
				names.Length == 0 ? "" : string.Format("AND TABLE_NAME IN ({0})", parameterNames)
			);
			var tables = new List<Table>();
			using (var rs = ExecuteReader(query, new MySqlParameter("@TableSchema", database))) {
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
WHERE TABLE_NAME = @TableName
AND TABLE_SCHEMA = @TableSchema";
			var c = new List<Column>();
			using (var rs = ExecuteReader(query, new MySqlParameter("@TableName", name), new MySqlParameter("@TableSchema", database))) {
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
