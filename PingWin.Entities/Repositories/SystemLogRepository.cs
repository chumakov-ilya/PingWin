using System;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class SystemLogRepository : ISystemLogRepository
	{
		[Inject]
		public IContextFactory ContextFactory { get; set; }

		public async Task SaveAsync(SystemLog log)
		{
			using (var context = ContextFactory.Create())
			{
				context.SystemLogs.Add(log);

				await context.SaveChangesAsync();
			}
		}

		public int Save(SystemLog log)
		{
			using (var context = ContextFactory.Create())
			{
				context.SystemLogs.Add(log);

				return context.SaveChanges();
			}
		}

		public async Task SaveAsync(Exception exception = null, string message = null)
		{
			var log = CreateLog(exception, message);

			await SaveAsync(log);
		}

		public int Save(Exception exception = null, string message = null)
		{
			var log = CreateLog(exception, message);

			return Save(log);
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