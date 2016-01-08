using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PingWin.Core
{
	public class DbConnectionRule: IRule
	{
		public string ConnectionString { get; private set; }

		public DbConnectionRule(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public async Task<bool> Execute()
		{
			Trace.WriteLine($"DbTester.Check START");

			var response = await ConnectionCanBeOpened(ConnectionString);

			bool ok = response;

			Trace.WriteLine($"{ok}: {ConnectionString}");

			return ok;
		}

		public static async Task<bool> ConnectionCanBeOpened(string constr)
		{
			using (var connection = new SqlConnection(constr))
			{
				try
				{
					connection.Open();
					connection.Close();
					return true;
				}
				catch (SqlException)
				{
					return false;
				}
			}
		}


		public string FailureDescription()
		{
			return $"inaccessible database [{ConnectionString}].";
		}
	}
}
