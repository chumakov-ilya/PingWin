﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class Runner
	{
		static Runner()
		{
			LogRepository = new LogRepository();
		}

		static LogRepository LogRepository { get; set; }

		public static void RunAll(JobRegistry registry)
		{
			var cancellationToken = new CancellationToken();

			var tasks = new List<Task>();

			foreach (var job in registry.Jobs)
			{
				Task task = Runner.RunOne(cancellationToken, job);

				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray(), cancellationToken);
		}

		public static async Task RunOne(CancellationToken cancellationToken, Job job)
		{
			await Task.Run(async () =>
			{
				int index = 0;
				while (index < 2)
				{
					Trace.WriteLine("iteration START");

					Func<Task<Log>> method = job.Rule.Execute;

					Log log = await Task.Run(method, cancellationToken);

					job.WriteSelfTo(log);
						
					LogRepository.Save(log);

					if (log.StatusEnum != StatusEnum.Success)
					{
						Trace.WriteLine("iteration TRIGGERS EXECUTION");

						var tasks = new List<Task>();

						foreach (var trigger in job.GetTriggers())
						{
							tasks.Add(trigger.Execute(log));
						}

						Task.WaitAll(tasks.ToArray());
					}

					await Task.Delay(job.CheckInterval, cancellationToken);

					index++;
				}
			}, cancellationToken);
		}
	}
}