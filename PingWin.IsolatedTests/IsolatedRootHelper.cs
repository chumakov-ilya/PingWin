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

			var names = new[] {"http", "db"};

			foreach (var name in names)
			{
				root.Records.Add(new JobRecord() { Name = name });
				root.AddJob(name, HttpRequestRule.Create(name));
			}

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