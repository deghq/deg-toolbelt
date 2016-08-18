// <file>
//  <license></license>
//  <owner name="Ian Escarro" email="ian.escarro@gmail.com"/>
// </file>

using System;
using System.Collections.Generic;

namespace Deg.Toolbelt
{
	public class Argument
	{
		public string Name { get; set; }
		public List<string> Options { get; set; }
		
		public Argument()
		{
			Options = new List<string>();
		}
		
		public static Arguments GetArguments(string[] args)
		{
			int i = 0;
			var arguments = new Arguments();
			while (i < args.Length) {
				if (args[i].StartsWith("-")) {
					var a = new Argument { Name = args[i] };
					arguments.Values.Add(a);
					i++;
					while (i < args.Length && !args[i].StartsWith("-")) {
						a.Options.Add(args[i]);
						i++;
					}
				} else {
					i++;
				}
			}
			return arguments;
		}
	}
	
	public class Arguments
	{
		public List<Argument> Values { get; set; }
		
		public Arguments()
		{
			Values = new List<Argument>();
		}
		
		public Argument GetArgument(string name)
		{
			foreach (var a in Values) {
				if (a.Name == name) {
					return a;
				}
			}
			return null;
		}
	}
}
