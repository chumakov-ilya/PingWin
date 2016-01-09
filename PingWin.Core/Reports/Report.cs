using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingWin.Entities;

namespace PingWin.Core
{
	public class Report
	{
		public Report()
		{
			RunInterval = TimeSpan.FromMinutes(1);

			ReportRepository = new ReportRepository();
		}

		private ReportRepository ReportRepository { get; set; }

		public async Task ExecuteAsync()
		{
			DateTime to = DateTime.Now;

			var list = ReportRepository.GetIntervalReport(to.AddHours(-1), to);

			await SendMail(list);
		}

		private async Task SendMail(ReportRowList list)
		{
			string subject = "PingWin regular report: Failures!";

			string body = GetBody(list);

			await Mailer.SendMail(subject, body);
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
					$"{row.JobName}: {row.Count} {row.First.DateTime.ToShortTimeString()} {row.Last.DateTime.ToShortTimeString()}");
			}

			return builder.ToString();
		}


		public TimeSpan RunInterval { get; }
	}
}