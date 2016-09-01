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
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length > 0) {
				string command = args[0];
				var arguments = Argument.GetArguments(args);
				if (command.StartsWith("generate")) {
					var service = new TableService(GetTableRepository(arguments.GetArgument("-sql").FirstOption(), arguments.GetArgument("-d").FirstOption()));
					var tables = service.FindTables(arguments.GetArgument("-t").OptionsToArray());
					
					var g = new Generator(service, tables, arguments);
					
					if (command == "generate-classes") {
						g.GenerateModels();
					} else if (command == "generate-repositories") {
						g.GenerateRepositories();
					} else if (command == "generate-scripts") {
						g.GenerateScripts();
					}
				}
			} else {
				Console.WriteLine(
					@"DEG toolbelt is a simple tool to generate basic model classes and repositories
from a database table(s).

  Usage:
    deg <command> [arguments...] [options...]

  Examples:
    deg generate-classes
    deg generate-repositories

  Arguments:
    -sql  Selects and assigns what SQL to grab the tables.
    -d    Choose the database to which generates the classes/repositories from.
    -n    Assigns the namespace for the model or repository classes.
    -t    Selects database table(s) separatected by space e.g. Customer Item.

  Further information:
    https://github.com/iescarro/deg-toolbelt"
				);
			}
		}
		
		static ITableRepository GetTableRepository(string sql, string database)
		{
			if (sql == "mysql") {
				return new MySqlTableRepository(database);
			} else {
				return new SqlTableRepository(database);
			}
		}
	}
}