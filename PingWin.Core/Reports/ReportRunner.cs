using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class ReportRunner
	{
		static ReportRunner()
		{
			LogRepository = new LogRepository();
			Reports = new List<Report>();
		}

		private static LogRepository LogRepository { get; set; }

		public static List<Report> Reports { get; set; }

		public static void RunAll()
		{
			var tasks = new List<Task>();

			//Task.Delay(GetTimeUntilNextHour()).Wait();

			foreach (var report in Reports)
			{
				Task task = RunOne(report);

				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());
		}

		public static TimeSpan GetTimeUntilNextHour()
		{
			return DateTime.Now.TruncateToHours().AddHours(1) - DateTime.Now;
		}

		public static async Task RunOne(Report report)
		{
			await Task.Run(async () =>
			{
				while (true)
				{
					await report.ExecuteAsync();

					await Task.Delay(report.RunInterval);
				}
			});
		}
	}
}