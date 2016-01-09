using System;

namespace PingWin.Core
{
	internal static class JobDefaultSettings
	{
		public static TimeSpan CheckInterval { get; } = TimeSpan.FromSeconds(5);
		public static TimeSpan FailureSilenceInterval { get; } = TimeSpan.FromSeconds(300);
	}
}