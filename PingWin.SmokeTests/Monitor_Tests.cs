using NUnit.Framework;
using PingWin.Core;

namespace PingWin.SmokeTests
{
	public class Monitor_Tests
	{
		[Test]
		public void Run_WhenCalledWithRealRoots_WaitsAndNoError()
		{
			var jobRoot = RealRootHelper.GetJobRootNotEmpty();

			ReportRoot reportRoot = RealRootHelper.GetReportRootNotEmpty();

			Monitor.Run(jobRoot, reportRoot);
		}
	}
}