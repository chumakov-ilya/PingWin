using System.Data.Common;
using System.Data.SqlClient;

namespace PingWin.Core
{
	public interface IConnectionFactory
	{
		DbConnection Create(string constr);
	}

	public class ConnectionFactory : IConnectionFactory
	{
		public DbConnection Create(string constr)
		{
			return new SqlConnection(constr);
		}
	}
}