using NUnit.Framework;

namespace PingWin.Core.Tests
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