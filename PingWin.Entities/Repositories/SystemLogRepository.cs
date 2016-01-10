using System;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class SystemLogRepository
	{
		public async Task SaveAsync(SystemLog log)
		{
			using (var context = new PingWinContext())
			{
				context.SystemLogs.Add(log);

				await context.SaveChangesAsync();
			}
		}

		public async Task SaveAsync(Exception exception = null, string message = null)
		{
			var log = CreateLog(exception, message);

			await SaveAsync(log);
		}

		public SystemLog CreateLog(Exception exception = null, string message = null)
		{
			var log = new SystemLog();

			log.DateTime = DateTime.Now.TruncateToSeconds();
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