﻿using System;
using System.Text;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Core.Triggers
{
	public class MailTrigger
	{
		public IRule Rule { get; set; }

		public async Task Execute(Log log)
		{
			string subject = "PingWin urgent report: Failure right now!";

			string body = GetBody(log);

			await Mailer.SendMail(subject, body);
		}

		private static string GetBody(Log log)
		{
			var builder = new StringBuilder();

			builder.AppendLine($"Failure: {log.ShortData}");
			builder.AppendLine();
			builder.AppendLine($"DateTime: {log.DateTime}");
			builder.AppendLine();
			builder.AppendLine("Technical information:");
			builder.AppendLine($"Status: {log.StatusEnum}");
			builder.AppendLine($"ShortData: {log.ShortData}");
			builder.AppendLine();
			builder.AppendLine($"FullData: {log.FullData}");
			builder.AppendLine();
			builder.AppendLine($"StackTrace: {log.StackTrace}");

			return builder.ToString();
		}
	}
}