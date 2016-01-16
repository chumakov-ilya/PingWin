using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class JobRoot_Tests
	{
		[Test]
		public async Task RunAllAsync_Test()
		{
			var root = RealRootHelper.GetJobRootNotEmpty();

			await root.RunAllAsync();
		}
	}
}