using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public interface IRule
	{
		Task<Log> ExecuteAsync();

		//string FailureDescription();
	}
}