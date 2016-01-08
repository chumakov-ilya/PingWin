using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class LogRepository
	{
		public void Save(Log log)
		{
			using (var context = new PingWinContext())
			{
				context.Logs.Add(log);

				context.SaveChanges();
			}
		} 
	}
}