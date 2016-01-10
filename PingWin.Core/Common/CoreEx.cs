using System;
using System.Threading.Tasks;

namespace PingWin.Core
{
	public class CoreEx
	{
		public static async Task DelayIfNeeded(TimeSpan expected, TimeSpan elapsed)
		{
			TimeSpan delta = expected - elapsed;

			await (delta.Ticks > 0 ? Task.Delay(delta) : Task.CompletedTask);
		}
	}
}