using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class Monitor_Tests
	{
		[Test]
		public void Run_WhenCalledWithRealRoots_WaitsAndNoError()
		{
			var jobRoot = GetJobRootNotEmpty();

			ReportRoot reportRoot = GetReportRootNotEmpty();

			Monitor.Run(jobRoot, reportRoot);

		}

		private static ReportRoot GetReportRootNotEmpty()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report())
				.Delayed(false);

			return root;
		}

		private static JobRoot GetJobRootNotEmpty()
		{
			var root = JobRoot.Default;

			root.AddJob("test-http-success", new HttpRequestRule("http://httpbin.org/post").SetMethod("POST").ExpectCode(200));

			root.AddJob("test-http-failure", new HttpRequestRule("http://httpbin.org/post"));

			root.AddJob("test-db-success", new DbConnectionRule("Server=.;Database=PostoryDb;Trusted_Connection=True;"));

			root.AddJob("test-db-failure", new DbConnectionRule("Server=.;Database=PostoryDbX;Trusted_Connection=True;"));

			foreach (Job job in root.GetJobs())
			{
				job
					//.AttachTrigger(new MailTrigger())
					.SetLogSuccess(true)
					.SetDebugSettings()
					;
			}

			return root;
		}
	}
}