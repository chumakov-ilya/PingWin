using System;

namespace PingWin.Core
{
	internal static class JobDefaultSettings
	{
		public static TimeSpan CheckInterval { get; } = TimeSpan.FromMinutes(1);
		public static TimeSpan FailureSilenceInterval { get; } = TimeSpan.FromMinutes(30);
	}
}