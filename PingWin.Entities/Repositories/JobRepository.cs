using System.Collections.Generic;
using System.Linq;
using Ninject;
using PingWin.Entities.Models;

namespace PingWin.Entities
{
	public class JobRepository
	{
		[Inject]
		public IContextFactory ContextFactory { get; set; }

		public List<JobRecord> GetJobRecords()
		{
			using (var context = ContextFactory.Create())
			{
				List<JobRecord> jobs = context.JobRecords.ToList();

				return jobs;
			}
		} 
	}
}