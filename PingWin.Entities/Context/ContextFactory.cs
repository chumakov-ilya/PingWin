namespace PingWin.Entities.Models
{
	public class ContextFactory : IContextFactory
	{
		public IPingWinContext Create()
		{
			return new PingWinContext();
		} 
	}
}