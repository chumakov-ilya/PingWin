using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class ReportRunner_Tests
	{
		[Test]
		public void RunAll_Test()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report());

			root.RunAll();
		} 
	}
}