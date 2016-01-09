using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingWin.Entities;
using PingWin.Entities.Models;

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
			var rows = ReportRepository.GetHourlyReport();

			await SendMail(rows);
		}

		private async Task SendMail(ReportRowList list)
		{
			string subject = "PingWin hourly report: Failures!";

			string body = GetBody(list);

			await Mailer.SendMail(subject, body);
		}

		private string GetBody(ReportRowList list)
		{
			var builder = new StringBuilder();

			builder.AppendLine($"PingWin has recorded {list.LogTotalCount} failures at last hour.");

			builder.AppendLine($"Interval begin: {DateTime.Now.AddHours(-1)}");
			builder.AppendLine($"Interval end: {DateTime.Now}");
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