using NUnit.Framework;

namespace PingWin.Core.Tests
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