using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace PingWin.Core
{
	public static class Monitor
	{
		public static void Run(JobRoot jobRoot, ReportRoot reportRoot = null)
		{
			if (jobRoot == null) throw new ArgumentException("jobRoot is required");

			var jobs = Task.Run(async () => await jobRoot.RunAllAsync());

			if (reportRoot != null)
			{
				AsyncContext.Run(async () => await reportRoot.RunAllAsync()); 
			}
			else
			{
				jobs.Wait();
			}
		}
	}
}