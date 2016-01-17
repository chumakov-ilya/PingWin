using System.Threading.Tasks;
using NUnit.Framework;
using PingWin.Core;

namespace PingWin.SmokeTests
{
	public class JobRoot_Tests
	{
		[Test]
		public async Task RunAllAsync_Test()
		{
			DefaultDiContainer.Kernel.Load(new [] { new MockModule()});

			var root = RealRootHelper.GetJobRootNotEmpty();

			await root.RunAllAsync();
		}
	}
}