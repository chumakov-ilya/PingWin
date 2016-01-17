using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class JobRunner
	{
		[Obsolete("Direct creation is denied.")]
		public JobRunner() {}

		[Inject]
		public ISystemLogRepository SystemLogRepository { get; set; }

		[Inject]
		public ILogRepository LogRepository { get; set; }

		public async Task RunAllAsync(List<Job> jobs)
		{
			try
			{
				var tasks = jobs.Select(job => Task.Run(async () => await RunCycleAsync(job))).ToList();

				await Task.WhenAll(tasks);
			}
			catch (Exception exception)
			{
				await SystemLogRepository.SaveAsync(exception, $"Fatal job runner error. Jobs will not be runned until restart.");
			}
		}

		public async Task RunCycleAsync(Job job)
		{
			var silence = new SilenceTime();

			while (true)
			{
				try
				{
					var elapsed = await RunJobAsync(job, silence);

					await CoreEx.DelayIfNeeded(job.CheckInterval, elapsed);
				}
				catch (Exception exception)
				{
					var minutes = TimeSpan.FromMinutes(10);

					await SystemLogRepository.SaveAsync(exception, 
						$"Unhandled job error. Job = [{job.Name}]. Job will be recycled in {minutes}. ");

					await Task.Delay(minutes); //TODO to config
				}
			}
		}

		[JobInterceptor]
		public virtual async Task<TimeSpan> RunJobAsync(Job job, SilenceTime silence)
		{
			//estimate report generation time to correct interval between runnings
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			Log log = await ExecuteRuleAsync(job);

			job.WriteSelfTo(log);

			bool success = log.StatusEnum == StatusEnum.Success;

			if (!success || job.LogSuccess) await LogRepository.SaveAsync(log);

			if (!success) await ExecuteTriggersAsync(job, silence, log);

			stopwatch.Stop();

			return stopwatch.Elapsed;
		}

		private async Task<Log> ExecuteRuleAsync(Job job)
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

		private async Task ExecuteTriggersAsync(Job job, SilenceTime silence, Log log)
		{
			try
			{
				if (silence.IsSilenceNow(log.DateTime))
				{
					Trace.WriteLine("Start triggers for a job: " + job.Name);

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
					Trace.WriteLine("Mute triggers for a job: " + job.Name);
					silence.IncreaseCounter();
				}
			}
			catch (Exception exception)
			{
				await SystemLogRepository.SaveAsync(exception, $"Unhandled trigger error. Job = [{job.Name}].");
			}
		}
	}
}