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
		public abstract string Name { get; }
		public string FileName { get { return Name + ".cs"; }}
		public string ModelName { get; set; }
		protected Table table;
		
		public Repository(Table table, string @namespace)
		{
			if (@namespace == "") {
				this.Namespace = table.Database;
			} else {
				this.Namespace = @namespace;
			}
			this.ModelName = table.Name;
			this.table = table;
		}
		
		public abstract string GetCreateScript();
	}
}
