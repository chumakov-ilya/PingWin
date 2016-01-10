using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PingWin.Entities;

namespace PingWin.Core
{
	public static class ReportRunner
	{
		static ReportRunner()
		{
			SystemLogRepository = new SystemLogRepository();
		}

		public static SystemLogRepository SystemLogRepository { get; set; }

		public static async Task RunAllAsync(List<Report> reports)
		{
			try
			{
				var tasks = new List<Task>();

				foreach (var report in reports)
				{
					if (report.DelayedStart) await Task.Delay(GetTimeUntilNextHour());

					tasks.Add(Task.Run(async () => await RunReportAsync(report)));
				}

				await Task.WhenAll(tasks);
			}
			catch (Exception exception)
			{
				await SystemLogRepository.SaveAsync(exception, $"Fatal report runner error. Reports are stopped until restart.");
			}
		}

		public static TimeSpan GetTimeUntilNextHour()
		{
			return DateTime.Now.TruncateToHours().AddHours(1) - DateTime.Now;
		}

		public static async Task RunReportAsync(Report report)
		{
			while (true)
			{
				try
				{
					//estimate report generation time to correct interval between runnings
					var stopwatch = new Stopwatch();
					stopwatch.Start();
					await report.ExecuteAsync();
					stopwatch.Stop();

					Trace.WriteLine(stopwatch.Elapsed);

					await CoreEx.DelayIfNeeded(report.RunInterval, stopwatch.Elapsed);
				}
				catch (Exception exception)
				{
					await SystemLogRepository.SaveAsync(exception, $"Unhandled report error.");
					
					await Task.Delay(TimeSpan.FromMinutes(10)); //TODO to config
				}
			}
		}
	}
}