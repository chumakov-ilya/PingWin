using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities;

namespace PingWin.Core
{
	public class ReportRoot
	{
		[Obsolete("Use Create method instead")]
		public ReportRoot()
		{

			Reports = new List<Report>();
		}

		[Inject]
		public ReportRunner Runner { get; set; }

		public static ReportRoot Default { get; } = Create();

		private List<Report> Reports { get; }

		public static ReportRoot Create()
		{
			return DefaultDiContainer.GetService<ReportRoot>();
		}

		public async Task RunAllAsync()
		{
			await Runner.RunAllAsync(Reports);
		}

		public Report AddReport(Report report)
		{
			Reports.Add(report);

			return report;
		}
	}
}