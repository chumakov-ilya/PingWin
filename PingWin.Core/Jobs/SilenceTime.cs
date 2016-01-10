using System;

namespace PingWin.Core
{
	internal class SilenceTime
	{
		public DateTime Until { get; set; } = DateTime.MinValue;

		private int Counter { get; set; } = 0;

		public bool IsSilenceNow(DateTime now)
		{
			return Until < now;
		}

		/// <summary>
		/// increase to 1 the count of muted messages.
		/// </summary>
		public void IncreaseCounter()
		{
			Counter++;
		}

		/// <summary>
		/// set count of muted messages to 0.
		/// </summary>
		public void ResetCounter()
		{
			Counter = 0;
		}

		public void UntilNow(DateTime dateTime)
		{
			Until = dateTime;
		}
	}
}