namespace PingWin.Entities.Models
{
	public interface IContextFactory
	{
		IPingWinContext Create();
	}
}