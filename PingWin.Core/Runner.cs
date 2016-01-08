using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PingWin.Core
{
	public class Runner
	{
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
			var timerInterval = 10000;

			await Task.Run(async () =>
			{
				int index = 0;
				while (index < 2)
				{
					Trace.WriteLine("iteration START");

					Func<Task<bool>> method = job.Rule.Execute;

					bool success = await Task.Run(method, cancellationToken);

					if (!success)
					{
						Trace.WriteLine("iteration TRIGGERS EXECUTION");

						var dt = DateTime.Now;

						var tasks = new List<Task>();

						foreach (var trigger in job.GetTriggers())
						{
							tasks.Add(trigger.Execute(dt));
						}

						Task.WaitAll(tasks.ToArray());
					}

					await Task.Delay(timerInterval, cancellationToken);

					index++;
				}
			}, cancellationToken);
		}
	}
}