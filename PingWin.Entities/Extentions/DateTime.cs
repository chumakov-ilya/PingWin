using System;

namespace PingWin.Entities
{
	public static class DateTimeExtentions
	{
		public static DateTime TruncateToSeconds(this DateTime dateTime) => TruncateTo(dateTime, TimeSpan.FromSeconds(1));
		public static DateTime TruncateToHours(this DateTime dateTime) => TruncateTo(dateTime, TimeSpan.FromHours(1));

		public static DateTime TruncateTo(this DateTime dateTime, TimeSpan timeSpan)
		{

			if (timeSpan == TimeSpan.Zero) return dateTime; 

			return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
		}
	}
}