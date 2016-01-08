using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class Mail_Tests
	{
		[Test]
		public async void MyMethod()
		{
			await Mailer.SendMail("ziga", "this is my test email body");
		}
	}
}