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
	public abstract class Class
	{
		public string Namespace { get; set; }
		public string Name { get; set; }
		public IList<Property> Properties { get; set; }
		public abstract string FileName { get; }
		
		public Class(Table table) : this(table, table.Database)
		{
		}
		
		public Class(Table table, string @namespace)
		{
			Namespace = @namespace != "" ? @namespace : table.Database;
			Name = table.Name.ToSingularize().ToPascalCase();
			Properties = new List<Property>();
			foreach (var c in table.Columns) {
				Properties.Add(new Property(c));
			}
		}
	}
	
	public class Property
	{
		public string Name { get; set; }
		public string Type { get; set; }
		
		public Property(Column c)
		{
			Name = c.Name.ToPascalCase();
			Type = GetPropertyType(c.Type);
		}
		
		string GetPropertyType(string type)
		{
			switch (type) {
				case "int":
				case "bigint":
					return "int";
				case "smalldatetime":
					return "DateTime?";
				case "decimal":
					return "decimal";
				case "bit":
					return "bool";
				case "image":
					return "byte[]";
				case "uniqueidentifier":
					return "Guid";
				case "double":
					return "double";
				default:
					return "string";
			}
		}
	}
}
