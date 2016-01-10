using System;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class LogRepository
	{
		public async Task SaveAsync(Log log)
		{
			using (var context = new PingWinContext())
			{
				context.Logs.Add(log);

				await context.SaveChangesAsync();
			}
		}


		public Log CreateLog(StatusEnum status, Exception exception = null, string message = null)
		{
			var log = new Log();

			log.DateTime = DateTime.Now.TruncateToSeconds();
			log.StatusEnum = status;
			log.Message = message;

			if (exception != null)
			{
				log.FullData = exception.ToString();
				log.StackTrace = exception.StackTrace; 
			}
			return log;
		}

	}
}