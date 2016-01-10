using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			var tasks = new List<Task>();

			foreach (var job in jobs)
			{
				Task task = RunJobAsync(job);

				tasks.Add(task);
			}

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

					Log log = await job.Rule.ExecuteAsync();

					job.WriteSelfTo(log);

					await LogRepository.SaveAsync(log);

					if (log.StatusEnum != StatusEnum.Success)
					{
						if (silence.IsSilenceNow(log.DateTime))
						{
							Trace.WriteLine("iteration TRIGGERS EXECUTION: " + job.Name);

							silence.SetUntil( log.DateTime + job.FailureSilenceInterval);

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

					await Task.Delay(job.CheckInterval);
				}
			});
		}
	}
}