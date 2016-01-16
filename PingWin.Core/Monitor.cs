using System;
using System.Threading.Tasks;
using Nito.AsyncEx;
using PingWin.Entities;

namespace PingWin.Core
{
	public static class Monitor
	{
		static Monitor()
		{
			SystemLogRepository = new SystemLogRepository();
		}

		public static SystemLogRepository SystemLogRepository { get; set; }

		public static void Run(JobRoot jobRoot, ReportRoot reportRoot = null)
		{
			try
			{
				if (jobRoot == null) throw new ArgumentException("jobRoot is required");

				var jobs = Task.Run(async () => await jobRoot.RunAllAsync());

				if (reportRoot != null)
				{
					//run reports syncronously
					AsyncContext.Run(async () => await reportRoot.RunAllAsync());
				}
				else
				{
					jobs.Wait();
				}
			}
			catch (Exception exception)
			{
				SystemLogRepository.Save(exception, $"Fatal PingWin error.");
				throw;
			}
		}
	}
}