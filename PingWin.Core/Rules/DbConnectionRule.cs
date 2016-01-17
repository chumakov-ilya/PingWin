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

		public async Task<Log> ExecuteAsync()
		{
			try
			{
				Trace.WriteLine($"DbTester.Check START");

				var ok = await ConnectionCanBeOpened(ConnectionString);

				Trace.WriteLine($"{ok}: {ConnectionString}");

				return LogRepository.CreateLog(StatusEnum.Success);
			}
			catch (SqlException exception)
			{
				var log = LogRepository.CreateLog(StatusEnum.Failure, exception);

				log.Message = FailureDescription();

				return log;
			}
		}

		public static DbConnectionRule Create(string connectionString)
		{
			var instance = DiContainer.GetService<DbConnectionRule>();

			instance.ConnectionString = connectionString;

			return instance;
		}

		public static async Task<bool> ConnectionCanBeOpened(string constr)
		{
			using (var connection = new SqlConnection(constr))
			{
				await connection.OpenAsync();

				return true;
			}
		}

		public string FailureDescription()
		{
			return $"Cann't open connection to [{ConnectionString}].";
		}
	}
}