using System;
using System.Threading.Tasks;

namespace PingWin.Core.Triggers
{
	public class MailTrigger
	{
		public async Task Execute(DateTime dt)
		{
			string subject = "Danger!";
			string body = $"Failure: {Rule.FailureDescription()}\nDateTime: {dt.ToString()}";

			await Mailer.SendMail(subject, body);
		}

		public IRule Rule { get; set; }
	}
}