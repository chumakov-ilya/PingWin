using PingWin.Core;

namespace PingWin.SmokeTests
{
	/// <summary>
	/// roots from this class should exist in db and work as expected
	/// </summary>
	public class RealRootHelper
	{
		public static ReportRoot GetReportRootNotEmpty()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report())
				.Delayed(false);

			return root;
		}

		public static JobRoot GetJobRootNotEmpty()
		{
			var root = JobRoot.Default;

			root.AddJob("test-http-success", HttpRequestRule.Create("http://httpbin.org/post").SetMethod("POST").ExpectCode(200));

			root.AddJob("test-http-failure", HttpRequestRule.Create("http://httpbin.org/post"));

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