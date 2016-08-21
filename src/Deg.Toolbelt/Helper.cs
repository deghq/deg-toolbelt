// <file>
//  <license></license>
//  <owner name="Ian Escarro" email="ian.escarro@gmail.com"/>
// </file>

using System;
using System.Text;

namespace Deg.Toolbelt
{
	public static class Helper
	{
		public static string ToCamelCase(this string str)
		{
			if (string.IsNullOrEmpty(str)) {
				return string.Empty;
			}
			
			var newString = new StringBuilder();
			foreach (var s in str.Split('_')) {
				char[] a = s.ToCharArray();
				a[0] = char.ToUpper(a[0]);
				newString.Append(new string(a));
			}

			char[] b = newString.ToString().ToCharArray();
			b[0] = char.ToLower(b[0]);
			
			return new string(b);
		}
		
		public static string ToPascalCase(this string str)
		{
			if (string.IsNullOrEmpty(str)) {
				return string.Empty;
			}
			
			var newString = new StringBuilder();
			foreach (var s in str.Split('_')) {
				char[] a = s.ToCharArray();
				a[0] = char.ToUpper(a[0]);
				newString.Append(new string(a));
			}

			return newString.ToString();
		}
		
		public static string ToSingularize(this string str)
		{
			if (str.EndsWith("ies")) {
				return str.TrimEnd(new char[] { 'i', 'e', 's' }) + "y";
			} else if (str.EndsWith("s")) {
				return str.TrimEnd('s');
			}
			return str;
		}
		
		public static string FirstOption(this Argument argument)
		{
			if (argument != null && argument.Options.Count > 0) {
				return argument.Options[0];
			} else {
				return "";
			}
		}
		
		public static string[] OptionsToArray(this Argument argument)
		{
			if (argument != null) {
				return argument.Options.ToArray();
			} else {
				return new string[0];
			}
		}
	}
}
