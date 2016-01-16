using System.Threading.Tasks;
using NUnit.Framework;
using PingWin.Core;

namespace PingWin.SmokeTests
{
	public class ReportRoot_Tests
	{
		[Test]
		public async Task RunAllAsync_Test()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report());

			await root.RunAllAsync();
		} 
	}
}