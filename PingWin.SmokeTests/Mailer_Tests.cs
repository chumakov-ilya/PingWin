using NUnit.Framework;
using PingWin.Core;

namespace PingWin.SmokeTests
{
	public class Mailer_Tests
	{
		[Test]
		public async void SendMailAsync_Test()
		{
			await Mailer.SendMailAsync("rush", "this is my test email body");
		}
	}
}