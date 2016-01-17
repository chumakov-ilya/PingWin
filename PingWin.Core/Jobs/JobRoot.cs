using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Ninject;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class JobRoot
	{
		[Obsolete("Use Create method instead")]
		[Inject]
		public JobRoot(JobRepository jobRepository)
		{
			Records = jobRepository.GetJobRecords();

			Jobs = new List<Job>();
		}

		[Inject]
		public JobRunner Runner { get; set; }

		public static JobRoot Default { get; } = Create();

		private List<JobRecord> Records { get; }
		private List<Job> Jobs { get; }


		public static JobRoot Create()
		{
			return DefaultDiContainer.GetService<JobRoot>();
		}

		public ReadOnlyCollection<Job> GetJobs()
		{
			return Jobs.AsReadOnly();
		}

		public Job AddJob(string name, IRule rule)
		{
			var record = Records.FirstOrDefault(r => r.Name == name);
			var jobRegistered = Jobs.Any(j => j.Name == name);

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

		//private void SetGlobalSettings(Job job)
		//{
		//	if (job.LogSuccess == null) job.LogSuccess = this.LogSuccess;
		//}

		public async Task RunAllAsync()
		{
			await Runner.RunAllAsync(Jobs);
		}
	}
}