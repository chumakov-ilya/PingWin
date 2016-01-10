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
		public delegate void SuccessTrigger();

		public Job(string name, IRule rule, int recordId)
		{
			RecordId = recordId;
			Name = name;
			Rule = rule;
			Triggers = new List<MailTrigger>();
			CheckInterval = JobDefaultSettings.CheckInterval;
			FailureSilenceInterval = JobDefaultSettings.FailureSilenceInterval;
		}

		public int RecordId { get; }

		public IRule Rule { get; }

		private List<MailTrigger> Triggers { get; }

		public string Name { get; set; }

		public TimeSpan CheckInterval { get; }

		public TimeSpan FailureSilenceInterval { get; }

		public void AttachTrigger(MailTrigger trigger)
		{
			trigger.Job = this;
			trigger.Rule = Rule;
			Triggers.Add(trigger);
		}

		public List<MailTrigger> GetTriggers()
		{
			return Triggers;
		}

		//public event EventHandler OnSuccess = delegate { };
		public void WriteSelfTo(Log log)
		{
			log.JobRecordId = RecordId;
		}
	}
}