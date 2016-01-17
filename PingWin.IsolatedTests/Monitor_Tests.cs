using NUnit.Framework;
using PingWin.Core;

namespace PingWin.IsolatedTests
{
	public class Monitor_Tests
	{
		[SetUp]
		public void SetUp()
		{
			DefaultDiContainer.Kernel.Load(new MockModule());
		}

		[Test]
		public void Run_WhenCalledWithRealRoots_WaitsAndNoError()
		{
			var jobRoot = DefaultDiContainer.GetService<JobRoot>();

			var reportRoot = DefaultDiContainer.GetService<ReportRoot>();

			Monitor.Run(jobRoot, reportRoot);
		}

		[Test]
		public void Run_WhenCalledWithNull_LogsErrorAndThrows()
		{
			Monitor.Run(null);
		}
	}
}