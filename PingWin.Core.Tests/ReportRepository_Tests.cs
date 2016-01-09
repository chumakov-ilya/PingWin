using System.Collections.Generic;
using NUnit.Framework;
using PingWin.Entities;

namespace PingWin.Core.Tests
{
	public class ReportRepository_Tests
	{
		[Test]
		public void GetHourlyReport_Test()
		{
			var repo = new ReportRepository();

			var rows = repo.GetHourlyReport();

			Assert.IsNotNull(rows);
		} 
	}
}