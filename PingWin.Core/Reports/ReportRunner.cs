using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities;

namespace PingWin.Core
{
	public class ReportRunner
	{
		[Obsolete("Direct creation is denied.")]
		public ReportRunner() { }

		[Inject]
		public ISystemLogRepository SystemLogRepository { get; set; }

		public async Task RunAllAsync(List<Report> reports)
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
				await SystemLogRepository.SaveAsync(exception, $"Fatal report runner error. Reports will not be runned until restart.");
			}
		}

		public TimeSpan GetTimeUntilNextHour()
		{
			return DateTime.Now.TruncateToHours().AddHours(1) - DateTime.Now;
		}

		public async Task RunReportAsync(Report report)
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
					var minutes = TimeSpan.FromMinutes(10);

					await SystemLogRepository.SaveAsync(exception, 
						$"Unhandled report error. Report will be recycled in {minutes}. ");

					await Task.Delay(minutes); //TODO to config
				}
			}
		}
	}
}