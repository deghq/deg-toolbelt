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
				if (command == "generate-classes") {
					var service = new TableService(FirstOption(arguments.GetArgument("-d")));
					var tables = service.FindTables(OptionsToArray(arguments.GetArgument("-t")));
					foreach (var t in tables) {
						Console.WriteLine("Converting table {0} to class...", t.Name);
						var c = new Class(t, FirstOption(arguments.GetArgument("-n")));
						
						Console.WriteLine("Writing {0}...", c.FileName);
						string path = Path.Combine(Directory.GetCurrentDirectory(), c.FileName);
						using (var w = new StreamWriter(path)) {
							w.WriteLine(c.ToString());
						}
						Console.WriteLine("{0} saved.", c.Name);
						Console.WriteLine();
					}
				}
			}
		}
		
		static string FirstOption(Argument argument)
		{
			if (argument != null && argument.Options.Count > 0) {
				return argument.Options[0];
			} else {
				return "";
			}
		}
		
		static string[] OptionsToArray(Argument argument)
		{
			if (argument != null) {
				return argument.Options.ToArray();
			} else {
				return new string[0];
			}
		}
	}
}