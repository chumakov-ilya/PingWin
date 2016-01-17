using System;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class MailTrigger
	{
		[Obsolete("Direct creation is denied.")]
		public MailTrigger() {}

		public static MailTrigger Create()
		{
			return DiContainer.GetService<MailTrigger>();
		}

		public ISilenceInfo SilenceInfo { get; private set; }
		public IRule Rule { get; set; }
		public IJob Job { get; set; }

		[Inject]
		public IMailer Mailer { get; set; }

		public async Task ExecuteAsync(Log log, ISilenceInfo silence = null)
		{
			SilenceInfo = silence;

			string subject = "PingWin: FAILURE! Right now!";

			string body = GetBody(log);

			await Mailer.SendMailAsync(subject, body);
		}

		private string GetBody(Log log)
		{
			var builder = new StringBuilder();

			builder.AppendLine($"Failure: {log.Message}");
			builder.AppendLine();
			builder.AppendLine($"DateTime: {log.DateTime}");
			builder.AppendLine();
			builder.AppendLine();
			builder.AppendLine("Technical information:");
			builder.AppendLine();
			builder.AppendLine($"Job: {Job.Name} (RecordId = {Job.RecordId})");
			builder.AppendLine();
			builder.AppendLine($"Status: {log.StatusEnum}");
			builder.AppendLine();
			builder.AppendLine($"ShortData: {log.Message}");
			builder.AppendLine();
			builder.AppendLine($"FullData: {log.FullData}");
			builder.AppendLine();
			builder.AppendLine($"StackTrace: {log.StackTrace}");
			builder.AppendLine();

			if (SilenceInfo != null)
			{
				if (SilenceInfo.Counter > 0)
				{
					builder.AppendLine(
						$"{SilenceInfo.Counter} messages before this were not sent due to Job.FailureSilenceInterval setting.");
				}
				builder.AppendLine(
					$"Next messages will not be sent until {SilenceInfo.Until} due to Job.FailureSilenceInterval setting.");
			}

			return builder.ToString();
		}
	}
}