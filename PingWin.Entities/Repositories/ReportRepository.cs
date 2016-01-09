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

		public List<ReportRow> GetHourlyReport()
		{
			DateTime hourAgo = DateTime.Now.AddHours(-1);

			using (var context = new PingWinContext())
			{
				var logs = context.Logs.Where(l => l.DateTime > hourAgo);

				List<int> jobIds = logs.Select(l => l.JobRecordId).ToList();

				if (!jobIds.Any()) return new List<ReportRow>();

				var jobs = JobRepository.GetJobRecords();

				var rows = new List<ReportRow>();

				foreach (int jobId in jobIds)
				{
					var row = new ReportRow();

					row.First = logs.OrderBy(l => l.DateTime).First();
					row.Last = logs.OrderByDescending(l => l.DateTime).First();
					row.Count = logs.Count();
					row.JobName = jobs.First(job => job.Id == jobId).Name;

					rows.Add(row);
				}

				return rows;
			}

		} 
	}
}