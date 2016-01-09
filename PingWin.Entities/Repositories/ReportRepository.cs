using System;
using System.Collections.Generic;
using System.Linq;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class ReportRepository
	{
		public JobRepository JobRepository { get; set; }

		public ReportRepository()
		{
			JobRepository = new JobRepository();
		}

		public ReportRowList GetHourlyReport()
		{
			DateTime hourAgo = DateTime.Now.AddHours(-1);

			using (var context = new PingWinContext())
			{
				var list = new ReportRowList();

				var logs = context.Logs.Where(l => l.DateTime > hourAgo);

				list.LogTotalCount = logs.Count();

				if (list.LogTotalCount == 0) return new ReportRowList();

				List<int> jobIds = logs.Select(l => l.JobRecordId).ToList();

				list.JobTotalCount = jobIds.Count;

				var jobs = JobRepository.GetJobRecords();

				foreach (int jobId in jobIds)
				{
					var row = new ReportRow();

					row.First = logs.OrderBy(l => l.DateTime).First();
					row.Last = logs.OrderByDescending(l => l.DateTime).First();
					row.Count = logs.Count();
					row.JobId = jobId;
					row.JobName = jobs.First(job => job.Id == jobId).Name;

					list.Rows.Add(row);
				}

				return list;
			}

		} 
	}
}