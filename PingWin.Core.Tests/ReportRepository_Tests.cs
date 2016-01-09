﻿using System;
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

			DateTime to = DateTime.Now;
			var rows = repo.GetIntervalReport(to.AddHours(-1), to);

			Assert.IsNotNull(rows);
		} 
	}
}