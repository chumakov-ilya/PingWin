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

		static LogRepository LogRepository { get; set; }

		public static void RunAll(List<Job> jobs)
		{
			var cancellationToken = new CancellationToken();

			var tasks = new List<Task>();

			foreach (var job in jobs)
			{
				Task task = JobRunner.RunOne(cancellationToken, job);

				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray(), cancellationToken);
		}

		public static async Task RunOne(CancellationToken cancellationToken, Job job)
		{
			await Task.Run(async () =>
			{
				DateTime silenceUntil = DateTime.MinValue;

				int index = 0;
				while (index < 2)
				{
					Trace.WriteLine("iteration START: " + job.Name);

					Func<Task<Log>> method = job.Rule.Execute;

					Log log = await Task.Run(method, cancellationToken);

					job.WriteSelfTo(log);
						
					LogRepository.Save(log);


					if (log.StatusEnum != StatusEnum.Success)
					{
						if (silenceUntil < log.DateTime)
						{
							Trace.WriteLine("iteration TRIGGERS EXECUTION: " + job.Name);

							silenceUntil = log.DateTime + job.FailureSilenceInterval;

							var tasks = new List<Task>();

							foreach (var trigger in job.GetTriggers())
							{
								tasks.Add(trigger.Execute(log));
							}

							Task.WaitAll(tasks.ToArray());
						}
						else
						{
							Trace.WriteLine("iteration TRIGGERS SILENCE: " + job.Name);
						}
					}

					await Task.Delay(job.CheckInterval, cancellationToken);

					index++;
				}
			}, cancellationToken);
		}
	}
}