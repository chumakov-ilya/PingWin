using NUnit.Framework;
using PingWin.Core;

namespace PingWin.SmokeTests
{
	public class Mailer_Tests
	{
		[Test]
		public async void SendMailAsync_Test()
		{
			var mailer = DefaultDiContainer.GetService<IMailer>();

			await mailer.SendMailAsync("rush", "this is my test email body");
		}
	}
}