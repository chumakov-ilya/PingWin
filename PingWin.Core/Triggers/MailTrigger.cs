using System.Text;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class MailTrigger
	{
		public ISilenceInfo SilenceInfo { get; private set; }
		public IRule Rule { get; set; }
		public IJob Job { get; set; }

		public async Task Execute(Log log, ISilenceInfo silence = null)
		{
			SilenceInfo = silence;

			string subject = "PingWin: FAILURE! Right now!";

			string body = GetBody(log);

			await Mailer.SendMail(subject, body);
		}

		private string GetBody(Log log)
		{
			var builder = new StringBuilder();

			builder.AppendLine($"Failure: {log.ShortData}");
			builder.AppendLine();
			builder.AppendLine($"DateTime: {log.DateTime}");
			builder.AppendLine();
			builder.AppendLine("Technical information:");
			builder.AppendLine($"Job: {Job.Name} (RecordId = {Job.RecordId})");
			builder.AppendLine($"Status: {log.StatusEnum}");
			builder.AppendLine($"ShortData: {log.ShortData}");
			builder.AppendLine();
			builder.AppendLine($"FullData: {log.FullData}");
			builder.AppendLine();
			builder.AppendLine($"StackTrace: {log.StackTrace}");

			if (SilenceInfo != null)
			{
				if (SilenceInfo.Counter > 0)
				{
					builder.AppendLine();
					builder.AppendLine($"{SilenceInfo.Counter} messages before this were not sent due to Job.FailureSilenceInterval setting.");
				}
				builder.AppendLine();
				builder.AppendLine($"Next messages will not be sent until {SilenceInfo.Until} due to Job.FailureSilenceInterval setting.");
			}

			return builder.ToString();
		}
	}
}