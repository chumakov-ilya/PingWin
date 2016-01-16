using NUnit.Framework;
using PingWin.Core;

namespace PingWin.SmokeTests
{
	public class DbConnectionRule_Tests
	{
		[Test]
		public void Execute_Test()
		{
			var rule = new DbConnectionRule("Server=.;Database=PostoryDb;Trusted_Connection=True;");

			while (true)
			{
				rule.ExecuteAsync().Wait(); 
			}
		}
	}
}