using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingWin.Core.Triggers;

namespace PingWin.Core
{
	public class Job
	{
		public delegate void SuccessTrigger();

		public Job(string name, IRule rule, int id)
		{
			Id = id;
			Name = name;
			Rule = rule;
			Triggers = new List<MailTrigger>();
			CheckInterval = JobDefaultSettings.CheckInterval;
			ErrorReportInterval = JobDefaultSettings.ErrorReportInterval;
		}

		public int Id { get; }

		public IRule Rule { get; }

		private List<MailTrigger> Triggers { get; }

		public string Name { get; set; }

		public int CheckInterval { get; set; }

		public int ErrorReportInterval { get; set; }

		public void AttachTrigger(MailTrigger trigger)
		{
			trigger.Rule = Rule;
			Triggers.Add(trigger);
		}

		public List<MailTrigger> GetTriggers()
		{
			return Triggers;
		}

		//public event EventHandler OnSuccess = delegate { };
	}
}