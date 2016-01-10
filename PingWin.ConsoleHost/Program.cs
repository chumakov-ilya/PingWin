using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;
using PingWin.Core;

namespace PingWin.ConsoleHost
{
	class Program
	{
		static void Main(string[] args)
		{
			Task.Run(() => RunReports());

			AsyncContext.Run(() => RunJobs());
		}

		private static async void RunJobs()
		{
			var root = JobRoot.Default;

			root.AddJob("test-http-success", 
				new HttpRequestRule("http://httpbin.org/post").SetMethod("POST").ExpectCode(200))
				.AttachTrigger(new MailTrigger())
					.SetCheckIntervalSeconds(1)
					.SetFailureSilenceIntervalSeconds(5);

			root.AddJob("test-http-failure", 
				new HttpRequestRule("http://httpbin.org/post"))
				.AttachTrigger(new MailTrigger())
					.SetCheckIntervalSeconds(1)
					.SetFailureSilenceIntervalSeconds(5);

			root.AddJob("test-db-success", new DbConnectionRule("Server=.;Database=PostoryDb;Trusted_Connection=True;"))
					.SetCheckIntervalSeconds(1)
					.SetFailureSilenceIntervalSeconds(5);
			root.AddJob("test-db-failure", new DbConnectionRule("Server=.;Database=PostoryDbX;Trusted_Connection=True;"))
					.SetCheckIntervalSeconds(1)
					.SetFailureSilenceIntervalSeconds(5);

			await root.RunAllAsync();
		}

		private static async void RunReports()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report());

			await root.RunAllAsync();
		}
	}
}
