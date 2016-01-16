using System.Threading.Tasks;
using NUnit.Framework;

namespace PingWin.Core.Tests
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