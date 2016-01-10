using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class JobRunner
	{
		static JobRunner()
		{
			LogRepository = new LogRepository();
		}

		private static LogRepository LogRepository { get; }

		public static async Task RunAllAsync(List<Job> jobs)
		{
			var tasks = jobs.Select(RunJobAsync).ToList();

			await Task.WhenAll(tasks);
		}

		public static async Task RunJobAsync(Job job)
		{
			await Task.Run(async () =>
			{
				var silence = new SilenceTime();

				while (true)
				{
					Trace.WriteLine("iteration START: " + job.Name);

					//estimate report generation time to correct interval between runnings
					var stopwatch = new Stopwatch();
					stopwatch.Start();

					Log log = await ExecuteRuleAsync(job);

					job.WriteSelfTo(log);

					bool success = log.StatusEnum == StatusEnum.Success;

					if (!success || job.LogSuccess) await LogRepository.SaveAsync(log);

					if (!success) await ExecuteTriggersAsync(job, silence, log);

					stopwatch.Stop();

					Trace.WriteLine(stopwatch.Elapsed);

					await CoreEx.DelayIfNeeded(job.CheckInterval, stopwatch.Elapsed);
				}
			});
		}

		private static async Task<Log> ExecuteRuleAsync(Job job)
		{
			try
			{
				return await job.Rule.ExecuteAsync();
			}
			catch (Exception exception)
			{
				string message = $"Unhandled job error. Job = [{job.Name}].";

				return LogRepository.CreateLog(StatusEnum.InternalError, exception, message);
			}
		}

		private static async Task ExecuteTriggersAsync(Job job, SilenceTime silence, Log log)
		{
			try
			{
				if (silence.IsSilenceNow(log.DateTime))
				{
					Trace.WriteLine("iteration TRIGGERS EXECUTION: " + job.Name);

					silence.SetUntil(log.DateTime + job.FailureSilenceInterval);

					var tasks = new List<Task>();

					foreach (var trigger in job.GetTriggers())
					{
						tasks.Add(trigger.ExecuteAsync(log, silence));
					}

					await Task.WhenAll(tasks);

					silence.ResetCounter();
				}
				else
				{
					Trace.WriteLine("iteration TRIGGERS SILENCE: " + job.Name);
					silence.IncreaseCounter();
				}
			}
			catch (Exception exception)
			{
				//string message = $"Unhandled trigger error. Job = [{job.Name}].";

				//var log = LogRepository.CreateLog(StatusEnum.InternalError, exception, message);

				//LogRepository.SaveAsync(log);
			}
		}
	}
}