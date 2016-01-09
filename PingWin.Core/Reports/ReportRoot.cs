using System.Collections.Generic;
using System.Threading;
using PingWin.Entities;

namespace PingWin.Core
{
	public class ReportRoot
	{
		private ReportRoot()
		{
			LogRepository = new LogRepository();
			Reports = new List<Report>();
		}

		public static ReportRoot Default { get; } = new ReportRoot();

		private LogRepository LogRepository { get; set; }

		private List<Report> Reports { get; set; }

		public void RunAll()
		{
			ReportRunner.RunAll(Reports);
		}

		public void AddReport(Report report)
		{
			Reports.Add(report);
		}
	}
}