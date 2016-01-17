using NUnit.Framework;
using PingWin.Core;

namespace PingWin.IsolatedTests
{
	public class Monitor_Tests
	{
		[SetUp]
		public void SetUp()
		{
			DiContainer.Kernel.Load(new MockModule());
		}

		[Test]
		public void Run_WhenCalledWithRealRoots_WaitsAndNoError()
		{
			var jobRoot = IsolatedRootHelper.GetJobRootNotEmpty();

			var reportRoot = IsolatedRootHelper.GetReportRootNotEmpty();

			Monitor.Run(jobRoot, reportRoot);
		}

		[Test]
		public void Run_WhenCalledWithNull_LogsErrorAndThrows()
		{
			Monitor.Run(null);
		}
	}
}