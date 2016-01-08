using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class DbConnectionRule: IRule
	{
		public string ConnectionString { get; private set; }
		public LogRepository LogRepository { get; set; }

		public DbConnectionRule(string connectionString)
		{
			ConnectionString = connectionString;
			LogRepository = new LogRepository();
		}

		public async Task<Log> Execute()
		{
			try
			{
				try
				{
					Trace.WriteLine($"DbTester.Check START");

					bool ok = await ConnectionCanBeOpened(ConnectionString);

					Trace.WriteLine($"{ok}: {ConnectionString}");

					return LogRepository.CreateLog(StatusEnum.Success);
				}
				catch (SqlException exception)
				{
					return LogRepository.CreateLog(StatusEnum.Failure, exception);
				}
			}
			catch (Exception exception)
			{
				return LogRepository.CreateLog(StatusEnum.InternalError, exception);
			}
		}

		public static async Task<bool> ConnectionCanBeOpened(string constr)
		{
			using (var connection = new SqlConnection(constr))
			{
				connection.Open();
				connection.Close();
				return true;
			}
		}

		public string FailureDescription()
		{
			return $"inaccessible database [{ConnectionString}].";
		}
	}
}
