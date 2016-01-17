using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class ReportRepository
	{
		[Inject]
		public JobRepository JobRepository { get; set; }

		[Inject]
		public IContextFactory ContextFactory { get; set; }

		public ReportRowList GetIntervalReport(DateTime begin, DateTime end)
		{
			const int success = (int)StatusEnum.Success;

			using (var context = ContextFactory.Create())
			{
				var list = new ReportRowList();
				list.Begin = begin;
				list.End = end;

				var query = context.Logs.Where(l => l.Status != success 
					&& begin < l.DateTime && l.DateTime <= end);

				list.LogTotalCount = query.Count();

				if (list.LogTotalCount == 0) return list;

				List<int> jobIds = query.Select(l => l.JobRecordId).Distinct().ToList();

				list.JobTotalCount = jobIds.Count;

				var jobs = JobRepository.GetJobRecords();

				foreach (int jobId in jobIds)
				{
					var row = new ReportRow();

					var queryPerJob = query.Where(l => l.JobRecordId == jobId);

					row.First = queryPerJob.OrderBy(l => l.DateTime).First();
					row.Last = queryPerJob.OrderByDescending(l => l.DateTime).First();
					row.Count = queryPerJob.Count();
					row.JobId = jobId;
					row.JobName = jobs.First(job => job.Id == jobId).Name;

					list.Rows.Add(row);
				}

				return list;
			}

		} 
	}
}