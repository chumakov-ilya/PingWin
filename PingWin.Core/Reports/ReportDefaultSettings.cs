using System;

namespace PingWin.Core
{
	internal static class ReportDefaultSettings
	{
		public static TimeSpan RunInterval { get; } = TimeSpan.FromMinutes(60);
	}
}