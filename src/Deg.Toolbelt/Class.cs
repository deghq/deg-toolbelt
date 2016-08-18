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
	public class BaseClass : Class
	{
		public BaseClass(string @namespace) : base(new Table { Name = "BaseModel" }, @namespace)
		{
		}
		
		public override string ToString()
		{
			return string.Format(@"using System;
	
namespace {0}.Models
{{
	public class {1}
	{{
		public {1}()
		{{
		}}
	}}
}}", Namespace, Name);
		}
	}
	
	public class Class
	{
		public string Namespace { get; set; }
		public string Name { get; set; }
		public IList<Property> Properties { get; set; }
		public string FileName { get { return Name + ".cs"; }}
		
		public Class(Table t) : this(t, t.Database)
		{
		}
		
		public Class(Table t, string @namespace)
		{
			Namespace = @namespace != "" ? @namespace : t.Database;
			Name = t.Name;
			Properties = new List<Property>();
			foreach (var c in t.Columns) {
				Properties.Add(new Property(c));
			}
		}
		
		public override string ToString()
		{
			string properties = "";
			foreach (var p in Properties) {
				properties += string.Format("		public {1} {0} {{ get; set; }}", p.Name, p.Type);
				properties += Environment.NewLine;
			}
			return string.Format(@"using System;
	
namespace {0}.Models
{{
	public class {2}
	{{
{1}
		public {2}()
		{{
		}}
	}}
}}", Namespace, properties, Name);
		}
	}
	
	public class Property
	{
		public string Name { get; set; }
		public string Type { get; set; }
		
		public Property(Column c)
		{
			Name = c.Name;
			Type = GetPropertyType(c.Type);
		}
		
		string GetPropertyType(string type)
		{
			switch (type) {
				case "int":
				case "bigint":
					return "int";
				case "smalldatetime":
					return "DateTime";
				case "decimal":
					return "decimal";
				case "bit":
					return "bool";
				case "image":
					return "byte[]";
				case "uniqueidentifier":
					return "Guid";
				default:
					return "string";
			}
		}
	}
}
