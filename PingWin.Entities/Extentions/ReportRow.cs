using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class ReportRow
	{
		public Log First { get; set; }
		public Log Last { get; set; }
		public int Count { get; set; }
		public string JobName { get; set; }
		public int JobId { get; set; }
	}
}