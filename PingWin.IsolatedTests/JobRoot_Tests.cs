using System.Threading.Tasks;
using NUnit.Framework;
using PingWin.Core;

namespace PingWin.IsolatedTests
{
	public class JobRoot_Tests
	{
		[Test]
		public async Task RunAllAsync_Test()
		{
			var root = DefaultDiContainer.GetService<JobRoot>();

			await root.RunAllAsync();
		}
	}
}