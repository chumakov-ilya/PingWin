using System;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public interface ILogRepository {
		Task SaveAsync(Log log);
		Log CreateLog(StatusEnum status, Exception exception = null, string message = null);
	}
}