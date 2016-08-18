// <file>
//  <license></license>
//  <owner name="Ian Escarro" email="ian.escarro@gmail.com"/>
// </file>

using System;

namespace Deg.Toolbelt
{
	public static class Helper
	{
		public static string ToCamelCase(this string s)
		{
			if (string.IsNullOrEmpty(s)) {
				return string.Empty;
			}

			char[] a = s.ToCharArray();
			a[0] = char.ToLower(a[0]);

			return new string(a);
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
