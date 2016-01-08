using System;
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


		public Log CreateLog(StatusEnum status, Exception exception = null)
		{
			var log = new Log();

			log.DateTime = DateTime.Now.TruncateToSeconds();
			log.StatusEnum = status;

			return log;
		}

	}
}