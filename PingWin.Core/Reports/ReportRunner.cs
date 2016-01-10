using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PingWin.Entities;

namespace PingWin.Core
{
	public static class ReportRunner
	{
		public static async Task RunAllAsync(List<Report> reports)
		{
			var tasks = new List<Task>();

			//Task.Delay(GetTimeUntilNextHour()).Wait();

			foreach (var report in reports)
			{
				Task task = RunReportAsync(report);

				tasks.Add(task);
			}

			await Task.WhenAll(tasks);
		}

		public static TimeSpan GetTimeUntilNextHour()
		{
			return DateTime.Now.TruncateToHours().AddHours(1) - DateTime.Now;
		}

		public static async Task RunReportAsync(Report report)
		{
			await Task.Run(async () =>
			{
				while (true)
				{
					//estimate report generation time to correct interval between runnings
					var stopwatch = new Stopwatch();
					stopwatch.Start();
					await report.ExecuteAsync();
					stopwatch.Stop();

					Trace.WriteLine(stopwatch.Elapsed);

					await Task.Delay(report.RunInterval - stopwatch.Elapsed);
				}
			});
		}
	}
}