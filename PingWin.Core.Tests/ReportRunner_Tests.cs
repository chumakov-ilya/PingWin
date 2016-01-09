using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class ReportRunner_Tests
	{
		[Test]
		public void RunAll()
		{
			ReportRunner.Reports.Add(new Report());

			ReportRunner.RunAll();
		} 
	}
}