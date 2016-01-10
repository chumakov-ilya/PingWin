using System.Threading.Tasks;
using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class Report_Tests
	{
		[Test]
		public async Task RunAll_Test()
		{
			var root = ReportRoot.Default;

			root.AddReport(new Report());

			await root.RunAllAsync();
		} 
	}
}