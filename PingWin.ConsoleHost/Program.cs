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

			root.SetLogSuccess(true);

			root.AddJob("test-http-success", 
				new HttpRequestRule("http://httpbin.org/post").SetMethod("POST").ExpectCode(200))
				.AttachTrigger(new MailTrigger())
					.SetDebugSettings();

			root.AddJob("test-http-failure", 
				new HttpRequestRule("http://httpbin.org/post"))
				.AttachTrigger(new MailTrigger())
					.SetDebugSettings();

			root.AddJob("test-db-success", new DbConnectionRule("Server=.;Database=PostoryDb;Trusted_Connection=True;"))
					.SetDebugSettings();

			root.AddJob("test-db-failure", new DbConnectionRule("Server=.;Database=PostoryDbX;Trusted_Connection=True;"))
					.SetDebugSettings();

			await root.RunAllAsync();
		}

		private static async void RunReports()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report())
				.Delayed(false);

			await root.RunAllAsync();
		}
	}
}
