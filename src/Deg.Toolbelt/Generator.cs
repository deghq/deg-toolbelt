// <file>
//  <license></license>
//  <owner name="Ian Escarro" email="ian.escarro@gmail.com"/>
// </file>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Deg.Toolbelt
{
	public class Generator
	{
		TableService service;
		List<Table> tables;
		Arguments arguments;
		
		public Generator(TableService service, List<Table> tables, Arguments arguments)
		{
			this.service = service;
			this.tables = tables;
			this.arguments = arguments;
		}
		
		public void GenerateModels(bool forClipboard)
		{
			string modelsDir = Path.Combine(Directory.GetCurrentDirectory(), "Models");
			if (!Directory.Exists(modelsDir)) {
				Console.WriteLine("Creating Models directory...");
				Directory.CreateDirectory(modelsDir);
				Console.WriteLine("Models directory created.");
			}
			
			bool forceOverwrite = arguments.GetArgument("-f") != null;
			
			Console.WriteLine("Writing BaseModel...");
			var bc = new BaseModelCSharpClass(arguments.GetArgument("-n").FirstOption());
			string path = Path.Combine(modelsDir, bc.FileName);
			if (!File.Exists(path) || forceOverwrite) {
				using (var w = new StreamWriter(path)) {
					w.WriteLine(bc.ToString());
				}
				Console.WriteLine("{0} saved.", bc.Name);
			} else {
				Console.WriteLine("Unable to overwrite {0}. Please use -f argument to overwrite the content.", bc.Name);
			}
			Console.WriteLine();
			
			StringBuilder clipboardText = new StringBuilder();
			foreach (var t in tables) {
				Console.WriteLine("Converting table {0} to class...", t.Name);
				var c = new CSharpClass(t, arguments.GetArgument("-n").FirstOption());
				
				if (forClipboard) {
					Console.WriteLine("Writing {0} class to clipboard...", c.Name);
					clipboardText.Append(c.ToString());
					Console.WriteLine("{0} class saved to clipboard.", c.Name);
//					path = Path.Combine(modelsDir, c.FileName);
//					if (!File.Exists(path) || forceOverwrite) {
//						using (var w = new StreamWriter(path)) {
//							w.WriteLine(c.ToString());
//						}
//						Console.WriteLine("{0} class saved.", c.Name);
//					} else {
//						Console.WriteLine("Unable to overwrite {0}. Please use -f argument to overwrite the content.", c.Name);
//					}
				} else {
					Console.WriteLine("Writing {0} class...", c.Name);
					path = Path.Combine(modelsDir, c.FileName);
					if (!File.Exists(path) || forceOverwrite) {
						using (var w = new StreamWriter(path)) {
							w.WriteLine(c.ToString());
						}
						Console.WriteLine("{0} class saved.", c.Name);
					} else {
						Console.WriteLine("Unable to overwrite {0}. Please use -f argument to overwrite the content.", c.Name);
					}
				}
				Console.WriteLine();
			}
			
			if (forClipboard) {
				Clipboard.SetText(clipboardText.ToString());
			}
		}
		
		public void GenerateRepositories(bool forClipboard)
		{
			string repositoriesDir = Path.Combine(Directory.GetCurrentDirectory(), "Repositories");
			if (!Directory.Exists(repositoriesDir)) {
				Console.WriteLine("Creating Repositories Directory...");
				Directory.CreateDirectory(repositoriesDir);
				Console.WriteLine("Repositories directory created.");
			}
			
			bool forceOverwrite = arguments.GetArgument("-f") != null;

			var br = service.GetBaseRepository(arguments.GetArgument("-n").FirstOption());
			
			StringBuilder clipboardText = new StringBuilder();
			string path = "";
			if (forClipboard) {
				Console.WriteLine("Writing {0} to clipboard...", br.Name);
				clipboardText.Append(br.ToString());
				Console.WriteLine("{0} saved to clipboard.", br.Name);
//				string path = Path.Combine(repositoriesDir, br.FileName);
//				if (!File.Exists(path) || forceOverwrite) {
//					using (var w = new StreamWriter(path)) {
//						w.WriteLine(br.ToString());
//					}
//					Console.WriteLine("{0} saved.", br.Name);
//				} else {
//					Console.WriteLine("Unable to overwrite {0}. Please use -f argument to overwrite the content.", br.Name);
//				}
			} else {
				Console.WriteLine("Writing {0}...", br.Name);
//				string path = Path.Combine(repositoriesDir, br.FileName);
				path = Path.Combine(repositoriesDir, br.FileName);
				if (!File.Exists(path) || forceOverwrite) {
					using (var w = new StreamWriter(path)) {
						w.WriteLine(br.ToString());
					}
					Console.WriteLine("{0} saved.", br.Name);
				} else {
					Console.WriteLine("Unable to overwrite {0}. Please use -f argument to overwrite the content.", br.Name);
				}
			}
			Console.WriteLine();
			
			foreach (var t in tables) {
				Console.WriteLine("Creating repository class for table {0}...", t.Name);
				var r = service.GetRepository(t, arguments.GetArgument("-n").FirstOption());
				Console.WriteLine("{0} created.", r.Name);
				
				if (forClipboard) {
					Console.WriteLine("Writing {0} class to clipboard...", r.Name);
					clipboardText.Append(r.ToString());
					Console.WriteLine("{0} class saved to clipboard.", r.Name);
//					path = Path.Combine(repositoriesDir, r.FileName);
//					if (!File.Exists(path) || forceOverwrite) {
//						using (var w = new StreamWriter(path)) {
//							w.WriteLine(r.ToString());
//						}
//						Console.WriteLine("{0} class saved.", r.Name);
//					} else {
//						Console.WriteLine("Unable to overwrite {0}. Please use -f argument to overwrite the content.", r.Name);
//					}
				} else {
					Console.WriteLine("Writing {0} class...", r.Name);
					path = Path.Combine(repositoriesDir, r.FileName);
					if (!File.Exists(path) || forceOverwrite) {
						using (var w = new StreamWriter(path)) {
							w.WriteLine(r.ToString());
						}
						Console.WriteLine("{0} class saved.", r.Name);
					} else {
						Console.WriteLine("Unable to overwrite {0}. Please use -f argument to overwrite the content.", r.Name);
					}
				}
				Console.WriteLine();
			}
			
			if (forClipboard) {
				Clipboard.SetText(clipboardText.ToString());
			}
		}
		
		public void GenerateScripts()
		{
			using (var w = new StreamWriter("test.sql")) {
				foreach (var t in tables) {
//					var r = service.GetRepository(t, arguments.GetArgument("-n").FirstOption());
					var r = new MySqlRepository(t, arguments.GetArgument("-n").FirstOption());
					
					Console.WriteLine(r.GetDropScript());
					Console.WriteLine(r.GetCreateScript());
					
					w.WriteLine(r.GetDropScript());
					w.WriteLine(r.GetCreateScript());
				}
			}
		}
	}
}
