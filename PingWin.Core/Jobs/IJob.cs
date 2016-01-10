using System;
using System.Collections.Generic;

namespace PingWin.Core
{
	public interface IJob {
		int RecordId { get; }
		IRule Rule { get; }
		string Name { get; set; }
		TimeSpan CheckInterval { get; }
		TimeSpan FailureSilenceInterval { get; }
		List<MailTrigger> GetTriggers();
	}
}