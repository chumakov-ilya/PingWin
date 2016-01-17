using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace PingWin.Entities.Models
{
	public interface IPingWinContext : IDisposable
	{
		IDbSet<JobRecord> JobRecords { get; set; }
		IDbSet<Log> Logs { get; set; }
		IDbSet<SystemLog> SystemLogs { get; set; }
		int SaveChanges();
		Task<int> SaveChangesAsync();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}