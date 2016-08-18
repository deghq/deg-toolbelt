// <file>
//  <license></license>
//  <owner name="Ian Escarro" email="ian.escarro@gmail.com"/>
// </file>

using System;
using System.Collections.Generic;
using System.IO;

namespace Deg.Toolbelt
{
	public class Generator
	{
		public Generator()
		{
		}
		
		public void GenerateModels(TableService service, List<Table> tables, Arguments arguments)
		{
			Console.WriteLine("Writing BaseModel...");
			var bc = new BaseClass(arguments.GetArgument("-n").FirstOption());
			string path = Path.Combine(Directory.GetCurrentDirectory(), bc.FileName);
			using (var w = new StreamWriter(path)) {
				w.WriteLine(bc.ToString());
			}
			Console.WriteLine("{0} saved.", bc.Name);
			Console.WriteLine();
			
			foreach (var t in tables) {
				Console.WriteLine("Converting table {0} to class...", t.Name);
				var c = new Class(t, arguments.GetArgument("-n").FirstOption());
				
				Console.WriteLine("Writing {0}...", c.Name);
				path = Path.Combine(Directory.GetCurrentDirectory(), c.FileName);
				using (var w = new StreamWriter(path)) {
					w.WriteLine(c.ToString());
				}
				Console.WriteLine("{0} saved.", c.Name);
				Console.WriteLine();
			}
		}
		
		public void GenerateRepositories(TableService service, List<Table> tables, Arguments arguments)
		{
			Console.WriteLine("Writing BaseSqlRepository...");
			var br = new BaseRepository(arguments.GetArgument("-n").FirstOption());
			string path = Path.Combine(Directory.GetCurrentDirectory(), br.FileName);
			using (var w = new StreamWriter(path)) {
				w.WriteLine(br.ToString());
			}
			Console.WriteLine("BaseSqlRepository saved.");
			Console.WriteLine();
			
			foreach (var t in tables) {
				Console.WriteLine("Creating repository class for table {0}...", t.Name);
				var r = new Repository(t, arguments.GetArgument("-n").FirstOption());
				
				Console.WriteLine("Writing {0}...", r.RepositoryName);
				path = Path.Combine(Directory.GetCurrentDirectory(), r.FileName);
				using (var w = new StreamWriter(path)) {
					w.WriteLine(r.ToString());
				}
				Console.WriteLine("{0} saved.", r.RepositoryName);
				Console.WriteLine();
			}
		}
	}
}
