using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class JobRegistry
	{
		public List<JobRecord> Records { get; set; }
		public List<Job> Jobs { get; set; }

		public JobRegistry()
		{
			var repository = new JobRepository();

			Records = repository.GetJobRecords();

			Jobs = new List<Job>();
		}

		public Job AddJob(string name, IRule rule)
		{
			var record = Records.FirstOrDefault(r => r.Name == name);
			bool jobRegistered = Jobs.Any(j => j.Name == name);

			if (record != null && !jobRegistered)
			{
				var job = new Job(name, rule, record.Id);

				Jobs.Add(job);

				return job;
			}

			if (jobRegistered) throw new CoreException($"Job with name '{name}' already registered.");

			if (record == null) throw new CoreException($"Job with name '{name}' cann't be registered. No record in db.");

			throw new CoreException($"Cann't register the job '{name}'");
		}
	}
}