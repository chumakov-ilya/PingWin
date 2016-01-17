using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities;

namespace PingWin.Core
{
	public class Report
	{
		[Obsolete("Direct creation is denied.")]
		public Report()
		{
			RunInterval = ReportDefaultSettings.RunInterval;
		}

		public static Report Create()
		{
			return DiContainer.GetService<Report>();
		}

		[Inject]
		public ReportRepository ReportRepository { get; set; }

		[Inject]
		private IMailer Mailer { get; set; }

		public async Task ExecuteAsync()
		{
			DateTime now = DateTime.Now;

			var list = ReportRepository.GetIntervalReport(now.AddHours(-1), now);

			if (list.LogTotalCount == 0) return;

			await SendMail(list);
		}

		private async Task SendMail(ReportRowList list)
		{
			string subject = "PingWin: Regular failure report!";

			string body = GetBody(list);

			await Mailer.SendMailAsync(subject, body);
		}

		private string GetBody(ReportRowList list)
		{
			var builder = new StringBuilder();

			builder.AppendLine($"PingWin has recorded {list.LogTotalCount} failures at confugured interval.");

			builder.AppendLine($"Interval begin: {list.Begin}");
			builder.AppendLine($"Interval end: {list.End}");
			builder.AppendLine();
			builder.AppendLine("Technical information below in the following format:");
			builder.AppendLine("<JOB_NAME>: <FAILURE_COUNT>    <FIRST_FAILURE_TIME>    <LAST_FAILURE_TIME>");
			builder.AppendLine();

			foreach (var row in list.Rows.OrderBy(row => row.JobName))
			{
				builder.AppendLine(
					$"{row.JobName}: {row.Count}    {row.First.DateTime.ToShortTimeString()}    {row.Last.DateTime.ToShortTimeString()}");
			}

			return builder.ToString();
		}


		public TimeSpan RunInterval { get; }

		public Report Delayed(bool value)
		{
			DelayedStart = value;
			return this;
		}

		public bool DelayedStart { get; set; }
	}
}