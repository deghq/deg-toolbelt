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
					var g = new Generator();
					var service = new TableService(arguments.GetArgument("-d").FirstOption());
					var tables = service.FindTables(arguments.GetArgument("-t").OptionsToArray());
					
					if (command == "generate-classes") {
						g.GenerateModels(service, tables, arguments);
					} else if (command == "generate-repositories") {
						g.GenerateRepositories(service, tables, arguments);
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
    -d  Choose the database to which generates the classes/repositories from.
    -n  Assigns the namespace for the model or repository classes.
    -t  Selects database table(s) separatected by space e.g. Customer Item.

  Further information:
    https://github.com/iescarro/deg-toolbelt"
				);
			}
		}
		
	}
}