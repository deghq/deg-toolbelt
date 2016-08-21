/*
 * Created by SharpDevelop.
 * User: Dev
 * Date: 8/21/2016
 * Time: 2:10 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace Deg.Toolbelt.Tests
{
	[TestFixture]
	public class Test1
	{
		[Test]
		public void TestMethod()
		{
			Assert.AreEqual(1, "dodong".Split('_').Length);
			Assert.AreEqual("DodongYanyan", "dodong_yanyan".ToPascalCase());
			Assert.AreEqual("dodongYanyan", "dodong_yanyan".ToCamelCase());
			Assert.AreEqual("Account", "accounts".ToSingularize().ToPascalCase());
			Assert.AreEqual("Company", "companies".ToSingularize().ToPascalCase());
		}
	}
}
