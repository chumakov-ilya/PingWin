using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class Job_Tests
	{
		[Test]
		public async Task RunAll_Test()
		{
			var root = JobRoot.Default;

			root.AddJob("pipe-wcf-ekey-v1", new HttpRequestRule("https://sandbox.webstore.mont.ru/B2bService.svc"));

			root.AddJob("test-db-postory", new DbConnectionRule("Server=.;Database=PostoryDbX;Trusted_Connection=True;"))
					.AttachTrigger(new MailTrigger())
					.SetCheckIntervalSeconds(1)
					.SetFailureSilenceIntervalSeconds(5);

			await root.RunAllAsync();
		}
	}
}