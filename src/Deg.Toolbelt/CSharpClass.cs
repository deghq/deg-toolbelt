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
	public class CSharpClass : Class
	{
		public override string FileName {
			get {
				return Name + ".cs";
			}
		}
		
		public CSharpClass(Table t) : this(t, t.Database)
		{
		}
		
		public CSharpClass(Table t, string @namespace) : base(t, @namespace)
		{
		}
		
		public override string ToString()
		{
			string properties = "";
			foreach (var p in Properties) {
				properties += string.Format("		public {1} {0} {{ get; set; }}", p.Name, p.Type);
				properties += Environment.NewLine;
			}
			string str = @"using System;
	
namespace __NAMESPACE__.Models
{
	public class __NAME__
	{
__PROPERTIES__
		public __NAME__()
		{
		}
	}
}";
			str = str.Replace("__NAMESPACE__", Namespace);
			str = str.Replace("__PROPERTIES__", properties);
			str = str.Replace("__NAME__", Name);
			return str;
		}
	}
	
	public class BaseModelCSharpClass : CSharpClass
	{
		public BaseModelCSharpClass(string @namespace) : base(new Table { Name = "BaseModel" }, @namespace)
		{
		}
		
		public override string ToString()
		{
			string str = @"using System;
	
namespace __NAMESPACE__.Models
{
	public class __NAME__
	{
		public __NAME__()
		{
		}
	}
}";
			str = str.Replace("__NAMESPACE__", Namespace);
			str = str.Replace("__NAME__", Name);
			return str;
		}
	}
}
