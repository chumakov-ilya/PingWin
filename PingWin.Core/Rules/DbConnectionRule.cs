using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class DbConnectionRule : IRule
	{
		[Obsolete("Use Create method instead")]
		public DbConnectionRule() {}

		public string ConnectionString { get; private set; }

		[Inject]
		public ILogRepository LogRepository { get; set; }

		//[RuleInterceptor]
		public virtual async Task<Log> ExecuteAsync()
		{
			try
			{
				Trace.WriteLine($"Start DbConnectionRule");

				await ConnectionCanBeOpened(ConnectionString);

				Trace.WriteLine($"Finish DbConnectionRule: Success.");

				return LogRepository.CreateLog(StatusEnum.Success);
			}
			catch (SqlException exception)
			{
				var log = LogRepository.CreateLog(StatusEnum.Failure, exception);

				log.Message = FailureDescription();

				Trace.WriteLine($"Finish DbConnectionRule: Failure.");

				return log;
			}
		}

		public static DbConnectionRule Create(string connectionString)
		{
			var instance = DiContainer.GetService<DbConnectionRule>();

			instance.ConnectionString = connectionString;

			return instance;
		}

		public static async Task ConnectionCanBeOpened(string constr)
		{
			using (var connection = new SqlConnection(constr))
			{
				await connection.OpenAsync();

				return;
			}
		}

		public string FailureDescription()
		{
			return $"Cann't open connection to [{ConnectionString}].";
		}
	}
}