using System;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public interface ISystemLogRepository {
		Task SaveAsync(SystemLog log);
		int Save(SystemLog log);
		Task SaveAsync(Exception exception = null, string message = null);
		int Save(Exception exception = null, string message = null);
		SystemLog CreateLog(Exception exception = null, string message = null);
	}
}