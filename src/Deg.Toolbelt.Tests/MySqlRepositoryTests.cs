// <file>
//  <license></license>
//  <owner name="Ian Escarro" email="ian.escarro@gmail.com"/>
// </file>
using System;
using NUnit.Framework;

namespace Deg.Toolbelt.Tests
{
	[TestFixture]
	public class MySqlRepositoryTests
	{
		[Test]
		public void TestMethod()
		{
			var t = new Table { Name = "Project" };
			t.Columns.Add(new Column { Name = "ProjectID", Type = "integer" });
			t.Columns.Add(new Column { Name = "Internal", Type = "varchar(255)" });
			
			Console.WriteLine(new MySqlRepository(t, "").GetCreateScript());
		}
	}
}
