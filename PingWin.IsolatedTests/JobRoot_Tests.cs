using System.Threading.Tasks;
using NUnit.Framework;
using PingWin.Core;

namespace PingWin.IsolatedTests
{
	public class JobRoot_Tests
	{
		[SetUp]
		public void SetUp()
		{
			DefaultDiContainer.Kernel.Load(new MockModule());
		}

		[Test]
		public async Task RunAllAsync_Test()
		{
			var root = DefaultDiContainer.GetService<JobRoot>();

			await root.RunAllAsync();
		}
	}
}