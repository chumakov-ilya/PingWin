using System.Collections.Generic;
using NUnit.Framework;
using PingWin.Core.Triggers;

namespace PingWin.Core.Tests
{
	public class Runner_Tests
	{
		[Test]
		public void RunAll_Test()
		{
			List<Job> jobs = new List<Job>();

			var job1 = new Job("", new WcfEndpointRule("https://sandbox.webstore.mont.ru/B2bService.svc"));
			var job2 = new Job("", new DbConnectionRule("Server=.;Database=PostoryDbX;Trusted_Connection=True;"));

			job2.AddTrigger(new MailTrigger());

			jobs.Add(job1);
			jobs.Add(job2);


			Runner.RunAll(jobs);
		}
	}
}