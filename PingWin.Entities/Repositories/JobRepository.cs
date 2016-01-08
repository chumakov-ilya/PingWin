using System.Collections.Generic;
using System.Linq;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class JobRepository
	{
		public List<JobRecord> GetJobRecords()
		{
			using (var context = new PingWinContext())
			{
				List<JobRecord> jobs = context.JobRecords.ToList();

				return jobs;
			}
		} 
	}
}