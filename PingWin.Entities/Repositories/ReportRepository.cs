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

		public ReportRowList GetIntervalReport(DateTime begin, DateTime end)
		{
			const int success = (int)StatusEnum.Success;

			using (var context = new PingWinContext())
			{
				var list = new ReportRowList();
				list.Begin = begin;
				list.End = end;

				var query = context.Logs.Where(l => l.Status != success 
					&& begin < l.DateTime && l.DateTime <= end);

				list.LogTotalCount = query.Count();

				if (list.LogTotalCount == 0) return new ReportRowList();

				List<int> jobIds = query.Select(l => l.JobRecordId).ToList();

				list.JobTotalCount = jobIds.Count;

				var jobs = JobRepository.GetJobRecords();

				foreach (int jobId in jobIds)
				{
					var row = new ReportRow();

					row.First = query.OrderBy(l => l.DateTime).First();
					row.Last = query.OrderByDescending(l => l.DateTime).First();
					row.Count = query.Count();
					row.JobId = jobId;
					row.JobName = jobs.First(job => job.Id == jobId).Name;

					list.Rows.Add(row);
				}

				return list;
			}

		} 
	}
}