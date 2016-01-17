using PingWin.Core;
using PingWin.Entities.Models;

namespace PingWin.IsolatedTests
{
	/// <summary>
	/// roots from this class will not touch db and any other external resoures
	/// </summary>
	public class IsolatedRootHelper
	{
		public static ReportRoot GetReportRootNotEmpty()
		{
			var root = ReportRoot.Default;

			root.AddReport(Report.Create())
				.Delayed(false);

			return root;
		}

		public static JobRoot GetJobRootNotEmpty()
		{
			var root = JobRoot.Default;

			CreateJob(root, "db", DbConnectionRule.Create("db"));
			CreateJob(root, "http", HttpRequestRule.Create("http"));

			foreach (Job job in root.GetJobs())
			{
				job
					.AttachTrigger(MailTrigger.Create())
					.SetLogSuccess(true)
					.SetDebugSettings()
					;
			}

			return root;
		}

		private static void CreateJob(JobRoot root, string name, IRule rule)
		{
			root.Records.Add(new JobRecord() {Name = name});
			root.AddJob(name, rule);
		}
	}
}