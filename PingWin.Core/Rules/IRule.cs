using System.Threading.Tasks;

namespace PingWin.Core
{
	public interface IRule
	{
		Task<bool> Execute();

		string FailureDescription();

	}
}