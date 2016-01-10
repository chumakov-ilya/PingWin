using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PingWin.Core
{
	public class Mailer
	{
		public static async Task SendMailAsync(string subject, string body)
		{
			var config = new ConfigurationHelper();

			var smtpClient = new SmtpClient(config.MailHost, config.MailPort);
			smtpClient.Credentials = new NetworkCredential(config.MailUserName, config.MailPassword);
			smtpClient.EnableSsl = true;

			MailMessage mail = new MailMessage(config.MailFrom, config.MailTo);
			mail.Subject = subject;
			mail.Body = body;

			await smtpClient.SendMailAsync(mail);
		}
	}
}