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
	public class Table
	{
		public string Database { get; set; }
		public string Name { get; set; }
		public IList<Column> Columns { get; set; }
		
		public Table()
		{
			Columns = new List<Column>();
		}
	}
	
	public class Column
	{
		public string Name { get; set; }
		public string Type { get; set; }
	}
}
