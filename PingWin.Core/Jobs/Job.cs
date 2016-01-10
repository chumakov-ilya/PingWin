using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class Job : IJob
	{
		public Job(string name, IRule rule, int recordId)
		{
			RecordId = recordId;
			Name = name;
			Rule = rule;
			Triggers = new List<MailTrigger>();
			CheckInterval = JobDefaultSettings.CheckInterval;
			FailureSilenceInterval = JobDefaultSettings.FailureSilenceInterval;
		}

		private List<MailTrigger> Triggers { get; }

		/// <summary>
		///     Default: false
		/// </summary>
		public bool LogSuccess { get; set; }

		public int RecordId { get; }

		public IRule Rule { get; }

		public string Name { get; set; }

		public TimeSpan CheckInterval { get; private set; }

		public TimeSpan FailureSilenceInterval { get; private set; }

		public List<MailTrigger> GetTriggers()
		{
			return Triggers;
		}

		public Job AttachTrigger(MailTrigger trigger)
		{
			trigger.Job = this;
			trigger.Rule = Rule;
			Triggers.Add(trigger);
			return this;
		}

		//public event EventHandler OnSuccess = delegate { };
		public void WriteSelfTo(Log log)
		{
			log.JobRecordId = RecordId;
		}

		public Job SetCheckIntervalSeconds(int value)
		{
			CheckInterval = TimeSpan.FromSeconds(value);

			return this;
		}

		public Job SetFailureSilenceIntervalSeconds(int value)
		{
			FailureSilenceInterval = TimeSpan.FromSeconds(value);

			return this;
		}

		public Job SetDebugSettings()
		{
			SetCheckIntervalSeconds(1)
				.SetFailureSilenceIntervalSeconds(5);

			return this;
		}

		/// <summary>
		///     Default: false
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public Job SetLogSuccess(bool value)
		{
			LogSuccess = value;
			return this;
		}
	}
}