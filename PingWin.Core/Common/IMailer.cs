using System.Threading.Tasks;

namespace PingWin.Core
{
	public interface IMailer
	{
		Task SendMailAsync(string subject, string body);
	}
}