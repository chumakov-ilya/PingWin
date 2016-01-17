using System;
using System.Data;
using System.Data.Common;
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

		[Inject]
		public IConnectionFactory ConnectionFactory { get; set; }

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

		public async Task ConnectionCanBeOpened(string constr)
		{
			using (DbConnection connection = ConnectionFactory.Create(constr))
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