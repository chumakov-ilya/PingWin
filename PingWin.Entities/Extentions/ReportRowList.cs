using System.Collections.Generic;

namespace PingWin.Entities
{
	public class ReportRowList
	{
		public ReportRowList()
		{
			Rows = new List<ReportRow>();
		}

		public List<ReportRow> Rows { get; set; }

		public int LogTotalCount { get; set; }
		public int JobTotalCount { get; set; }
	}
}