using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingWin.Core;

namespace PingWin.ConsoleHost
{
	class Program
	{
		static void Main(string[] args)
		{
			RunReports();

			RunJobs();
		}

		private static void RunJobs()
		{
			var root = JobRoot.Default;

			root.AddJob("test-http-success", 
				new HttpRequestRule("http://httpbin.org/post").SetMethod("POST").ExpectCode(200))
				.AttachTrigger(new MailTrigger());

			root.AddJob("test-http-failure", 
				new HttpRequestRule("http://httpbin.org/post"))
				.AttachTrigger(new MailTrigger());

			root.AddJob("test-db-success", new DbConnectionRule("Server=.;Database=PostoryDb;Trusted_Connection=True;"));
			root.AddJob("test-db-failure", new DbConnectionRule("Server=.;Database=PostoryDbX;Trusted_Connection=True;"));

			//root.RunAll();
		}

		private static void RunReports()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report());

			Task.Run(() => root.RunAllAsync());
		}
	}
}
