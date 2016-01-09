using System.Collections.Generic;
using NUnit.Framework;
using PingWin.Core.Triggers;

namespace PingWin.Core.Tests
{
	public class Job_Tests
	{
		[Test]
		public void RunAll_Test()
		{
			var root = JobRoot.Default;

			//root.AddJob("pipe-wcf-ekey-v1", new WcfEndpointRule("https://sandbox.webstore.mont.ru/B2bService.svc"));
			root.AddJob("test-db-postory", new DbConnectionRule("Server=.;Database=PostoryDbX;Trusted_Connection=True;"))
					.AttachTrigger(new MailTrigger());

			root.RunAll();
		}
	}
}