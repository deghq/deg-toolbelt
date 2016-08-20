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
	public abstract class Repository
	{
		public string Namespace { get; set; }
		public string Name { get; set; }
		public string FileName { get { return Name + ".cs"; }}
		public string ModelName { get; set; }
		protected Table table;
		
		public Repository(Table t, string @namespace)
		{
			this.Namespace = @namespace;
			this.ModelName = t.Name;
			this.Name = "Sql" + ModelName + "Repository";
			this.table = t;
		}
	}
	
	public class SqlRepository : Repository
	{
//		public string Namespace { get; set; }
//		public string Name { get; set; }
//		public string FileName { get { return Name + ".cs"; }}
//		public string ModelName { get; set; }
//		protected Table table;
		
		public SqlRepository(Table t, string @namespace) : base(t, @namespace)
		{
//			this.Namespace = @namespace;
//			this.ModelName = t.Name;
//			this.Name = "Sql" + ModelName + "Repository";
//			this.table = t;
		}
		
		public override string ToString()
		{
			string str = @"using System;
using __NAMESPACE__.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
	
namespace __NAMESPACE__.Repositories
{
	public class __NAME__ : BaseSqlRepository<__MODEL_NAME__>
	{
		public __NAME__()
		{
		}
		
		public override void Save(__MODEL_NAME__ __VARIABLE__)
		{
			string query = @""
INSERT INTO __MODEL_NAME__(
__COLUMNS__
)
VALUES(
__PARAMETERIZED_COLUMNS__
)"";
			ExecuteNonQuery(
				query,
__SQL_PARAMETERS__
			);
		}
		
		public override void Update(__MODEL_NAME__ __VARIABLE__, int id)
		{
			string query = @""
UPDATE __MODEL_NAME__ SET
__COLUMNS_AND_PARAMETERIZED_COLUMNS__
WHERE __MODEL_NAME__ID = @__MODEL_NAME__ID"";
			ExecuteNonQuery(
				query,
__SQL_PARAMETERS__
			);
		}
		
		public override void Delete(int id)
		{
			string query = @""
DELETE FROM __MODEL_NAME__
WHERE __MODEL_NAME__ID = @__MODEL_NAME__ID"";
			ExecuteNonQuery(
				query,
				new SqlParameter(""@__MODEL_NAME__ID"", id)
			);
		}
		
		public override __MODEL_NAME__ Read(int id)
		{
			string query = @""
SELECT __COLUMNS__
FROM __MODEL_NAME__
WHERE __MODEL_NAME__ID = @__MODEL_NAME__ID"";
			__MODEL_NAME__ __VARIABLE__ = null;
			using (var rs = ExecuteReader(query, new SqlParameter(""@__MODEL_NAME__ID"", id))) {
				if (rs.Read()) {
					__VARIABLE__ = new __MODEL_NAME__ {
__ASSIGNED_PROPERTIES__
					};
				}
			}
			return __VARIABLE__;
		}
		
		public override IList<__MODEL_NAME__> FindAll()
		{
			string query = @""
SELECT __COLUMNS__
FROM __MODEL_NAME__"";
			var __VARIABLE__s = new List<__MODEL_NAME__>();
			using (var rs = ExecuteReader(query)) {
				while (rs.Read()) {
					__VARIABLE__s.Add(new __MODEL_NAME__ {
__ASSIGNED_PROPERTIES__
					});
				}
			}
			return __VARIABLE__s;
		}
	}
}";
			str = str.Replace("__NAMESPACE__", Namespace);
			str = str.Replace("__NAME__", Name);
			str = str.Replace("__MODEL_NAME__", ModelName);
			str = str.Replace("__VARIABLE__", ModelName.ToCamelCase());
			
			string columns = "";
			int i = 1;
			foreach (var c in table.Columns) {
				columns += "	" + c.Name;
				columns += i++ < table.Columns.Count ? ", " + Environment.NewLine : "";
			}
			str = str.Replace("__COLUMNS__", columns);
			
			string parameterizedColumns = "";
			i = 1;
			foreach (var c in table.Columns) {
				parameterizedColumns += "	@" + c.Name;
				parameterizedColumns += i++ < table.Columns.Count ? ", " + Environment.NewLine : "";
			}
			str = str.Replace("__PARAMETERIZED_COLUMNS__", parameterizedColumns);
			
			string sqlParameters = "";
			i = 1;
			foreach (var c in table.Columns) {
				sqlParameters += string.Format(@"				new SqlParameter(""@{0}"", {1}.{2})", c.Name, ModelName.ToCamelCase(), c.Name);
				sqlParameters += i++ < table.Columns.Count ? "," + Environment.NewLine : "";
			}
			str = str.Replace("__SQL_PARAMETERS__", sqlParameters);
			
			string columnsAndParameterizedColumns = "";
			i = 1;
			foreach (var c in table.Columns) {
				columnsAndParameterizedColumns += string.Format("	{0} = @{0}", c.Name);
				columnsAndParameterizedColumns += i++ < table.Columns.Count ? "," + Environment.NewLine : "";
			}
			str = str.Replace("__COLUMNS_AND_PARAMETERIZED_COLUMNS__", columnsAndParameterizedColumns);
			
			string assignedProperties = "";
			i = 1;
			foreach (var c in table.Columns) {
				assignedProperties += string.Format(@"						{0} = {1}", c.Name, lalala(c, i));
				assignedProperties += i++ < table.Columns.Count ? "," + Environment.NewLine : "";
			}
			str = str.Replace("__ASSIGNED_PROPERTIES__", assignedProperties);
			
			return str;
		}
		
		string lalala(Column c, int i)
		{
			switch (c.Type) {
					case "int":
				case "bigint":
					return string.Format("GetInt32(rs, {0})", i - 1);
				case "smalldatetime":
					return string.Format("GetDateTime(rs, {0})", i - 1);
				case "decimal":
					return string.Format("GetDecimal(rs, {0})", i - 1);
				case "bit":
					return string.Format("GetBoolean(rs, {0})", i - 1);
				case "image":
					return "byte[]";
				case "uniqueidentifier":
					return string.Format("GetGuid(rs, {0})", i - 1);
				default:
					return string.Format("GetString(rs, {0})", i - 1);
			}
		}
	}
	
	public class BaseSqlRepository : SqlRepository
	{
		public BaseSqlRepository(string @namespace) : base(new Table { Name = "Base" }, @namespace)
		{
		}
		
		public override string ToString()
		{
			string str = @"using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
	
namespace __NAMESPACE__.Repositories
{
	public class BaseSqlRepository<T>
	{
		SqlConnection con;
		
		public BaseSqlRepository()
		{
		}
		
		public virtual void Save(T t)
		{
			throw new NotImplementedException();
		}
		
		public virtual void Update(T t, int id)
		{
			throw new NotImplementedException();
		}
		
		public virtual void Delete(int id)
		{
			throw new NotImplementedException();
		}
		
		public virtual T Read(int id)
		{
			throw new NotImplementedException();
		}
		
		public virtual IList<T> FindAll()
		{
			throw new NotImplementedException();
		}
		
		SqlConnection CreateConnection(string connectionName)
		{
			if (connectionName == """") {
				con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[""database""]].ConnectionString);
			} else if (ConfigurationManager.ConnectionStrings[connectionName] != null) {
				con = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
			} else {
				con = new SqlConnection(ConfigurationManager.AppSettings[connectionName]);
			}
			return con;
		}
		
		protected void ExecuteNonQuery(string query, params SqlParameter[] parameters)
		{
			ExecuteNonQuery(query, ""SqlConnection"", parameters);
		}
		
		protected void ExecuteNonQuery(string query, string connectionName, params SqlParameter[] parameters)
		{
			con = CreateConnection(connectionName);
			OpenConnection();
			SqlCommand cmd = new SqlCommand(query, con);
			foreach (var p in parameters) {
				if (p.Value == null) {
					p.Value = DBNull.Value;
				}
				cmd.Parameters.Add(p);
			}
			cmd.ExecuteNonQuery();
			CloseConnection();
		}

        protected object ExecuteScalar(string query, string connectionName, params SqlParameter[] parameters)
        {
            con = CreateConnection(connectionName);
            OpenConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            foreach (var p in parameters) {
                if (p.Value == null) {
                    p.Value = DBNull.Value;
                }
                cmd.Parameters.Add(p);
            }
            object obj = cmd.ExecuteScalar();
            CloseConnection();
            return obj;
        }
		
		protected SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
		{
			return ExecuteReader(query, ""SqlConnection"", parameters);
		}
		
		protected SqlDataReader ExecuteReader(string query, string connectionName, params SqlParameter[] parameters)
		{
			con = CreateConnection(connectionName);
			OpenConnection();
			SqlCommand cmd = new SqlCommand(query, con);
			foreach (var p in parameters) {
				if (p.Value == null) {
					p.Value = DBNull.Value;
				}
				cmd.Parameters.Add(p);
			}
			return cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}

        protected DateTime? GetDateTime(SqlDataReader rs, int index, DateTime? def)
        {
            return rs.IsDBNull(index) ? def : (DateTime?)rs.GetDateTime(index);
        }
		
		protected DateTime? GetDateTime(SqlDataReader rs, int index)
		{
            return GetDateTime(rs, index, null);
		}
		
		protected void SetDateTime(DateTime date, SqlDataReader rs, int index)
		{
			if (rs.IsDBNull(index)) {
				date = rs.GetDateTime(index);
			}
		}
		
		protected string GetString(SqlDataReader rs, int index)
		{
			return GetString(rs, index, null);
		}
		
		protected string GetString(SqlDataReader rs, int index, string def)
		{
			return rs.IsDBNull(index) ? def : rs.GetString(index);
		}
		
		protected string GetString(SqlDataReader rs, int index, string check, string def)
		{
			bool condition = GetString(rs, index, check) == check;
			return condition ? def : rs.GetString(index);
		}
		
		protected bool GetBoolean(SqlDataReader rs, int index)
		{
			return rs.IsDBNull(index) ? false : rs.GetBoolean(index);
		}
		
		protected float GetFloat(SqlDataReader rs, int index)
		{
			return GetFloat(rs, index, 0);
		}
		
		protected float GetFloat(SqlDataReader rs, int index, float def)
		{
			return rs.IsDBNull(index) ? def : rs.GetFloat(index);
		}
		
		protected int GetInt32(SqlDataReader rs, int index)
		{
			return GetInt32(rs, index, 0);
		}
		
		protected int GetInt32(SqlDataReader rs, int index, int def)
		{
			return rs.IsDBNull(index) ? def : rs.GetInt32(index);
		}
		
		protected int? GetInt32Nullable(SqlDataReader rs, int index)
		{
			return GetInt32Nullable(rs, index, null);
		}
		
		protected int? GetInt32Nullable(SqlDataReader rs, int index, int? def)
		{
			return rs.IsDBNull(index) ? def : rs.GetInt32(index);
		}
		
		protected double GetDouble(SqlDataReader rs, int index)
		{
			return GetDouble(rs, index, 0);
		}
		
		protected double GetDouble(SqlDataReader rs, int index, double def)
		{
			return rs.IsDBNull(index) ? def : rs.GetDouble(index);
		}
		
		protected decimal GetDecimal(SqlDataReader rs, int index)
		{
			return GetDecimal(rs, index, 0);
		}
		
		protected decimal GetDecimal(SqlDataReader rs, int index, decimal def)
		{
			return rs.IsDBNull(index) ? def : rs.GetDecimal(index);
		}
		
		protected Guid GetGuid(SqlDataReader rs, int index)
		{
			return GetGuid(rs, index, new Guid());
		}
		
		protected Guid GetGuid(SqlDataReader rs, int index, Guid def)
		{
			return rs.IsDBNull(index) ? def : rs.GetGuid(index);
		}
		
		protected void CloseConnection()
		{
			if (con != null && con.State == ConnectionState.Open) {
				con.Close();
			}
		}
		
		protected void OpenConnection()
		{
			if (con != null && con.State == ConnectionState.Closed) {
				con.Open();
			}
		}
	}
}";
			str = str.Replace("__NAMESPACE__", Namespace);
			return str;
		}
	}
}
