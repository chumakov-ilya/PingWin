using System;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class LogRepository : ILogRepository
	{
		[Inject]
		public IContextFactory ContextFactory { get; set; }

		public async Task SaveAsync(Log log)
		{
			using (var context = ContextFactory.Create())
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