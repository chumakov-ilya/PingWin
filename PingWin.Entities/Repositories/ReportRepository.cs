using System;
using System.Collections.Generic;
using System.Linq;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class ReportRepository
	{
		public void MyMethod()
		{
			DateTime hourAgo = DateTime.Now.AddHours(-1);

			using (var context = new PingWinContext())
			{
				var logs = context.Logs.Where(l => l.DateTime > hourAgo);

				List<int> jobIds = logs.Select(l => l.JobRecordId).ToList();

				foreach (int jobId in jobIds)
				{
					var first = logs.OrderBy(l => l.DateTime).First();
					var last = logs.OrderBy(l => l.DateTime).Last();
					int count = logs.Count();
				}
				context.SaveChanges();
			}

		} 
	}
}