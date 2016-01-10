﻿using System;
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

		public static void RunAll(List<Job> jobs)
		{
			var cancellationToken = new CancellationToken();

			var tasks = new List<Task>();

			foreach (var job in jobs)
			{
				Task task = RunOne(cancellationToken, job);

				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray(), cancellationToken);
		}

		public static async Task RunOne(CancellationToken cancellationToken, Job job)
		{
			await Task.Run(async () =>
			{
				var silence = new SilenceTime();

				while (true)
				{
					Trace.WriteLine("iteration START: " + job.Name);

					Func<Task<Log>> method = job.Rule.Execute;

					Log log = await Task.Run(method, cancellationToken);

					job.WriteSelfTo(log);

					LogRepository.Save(log);


					if (log.StatusEnum != StatusEnum.Success)
					{
						if (silence.IsSilenceNow(log.DateTime))
						{
							Trace.WriteLine("iteration TRIGGERS EXECUTION: " + job.Name);

							silence.SetUntil( log.DateTime + job.FailureSilenceInterval);

							var tasks = new List<Task>();

							foreach (var trigger in job.GetTriggers())
							{
								tasks.Add(trigger.Execute(log, silence));
							}

							Task.WaitAll(tasks.ToArray());

							silence.ResetCounter();
						}
						else
						{
							Trace.WriteLine("iteration TRIGGERS SILENCE: " + job.Name);
							silence.IncreaseCounter();
						}
					}

					await Task.Delay(job.CheckInterval, cancellationToken);
				}
			}, cancellationToken);
		}
	}
}