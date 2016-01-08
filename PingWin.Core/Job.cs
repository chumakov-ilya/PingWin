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
		public IRule Rule { get; private set; }
		List<MailTrigger> Triggers { get; set; }

		public Job(string name, IRule rule)
		{
			Rule = rule;
			Triggers = new List<MailTrigger>();
			CheckInterval = JobDefaultSettings.CheckInterval;
			ErrorReportInterval = JobDefaultSettings.ErrorReportInterval;
		}

		public void AddTrigger(MailTrigger trigger)
		{
			trigger.Rule = this.Rule;
			Triggers.Add(trigger);
		}

		public List<MailTrigger> GetTriggers()
		{
			return Triggers;
		}
		public delegate void SuccessTrigger();

		public event EventHandler OnSuccess = delegate { };

		public int CheckInterval { get; set; }

		public int ErrorReportInterval { get; set; }
	}

	internal static class JobDefaultSettings
	{
		public static int CheckInterval { get; } = 1;

		public static int ErrorReportInterval { get; } = 300;
	}
}
