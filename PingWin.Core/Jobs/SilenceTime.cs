using System;

namespace PingWin.Core
{
	public interface ISilenceInfo
	{
		DateTime Until { get; }
		int Counter { get; }
		bool IsSilenceNow(DateTime now);
	}

	public class SilenceTime : ISilenceInfo
	{
		public DateTime Until { get; private set; } = DateTime.MinValue;

		public int Counter { get; private set; } = 0;

		public bool IsSilenceNow(DateTime now)
		{
			return Until < now;
		}

		/// <summary>
		///     increase to 1 the count of muted messages.
		/// </summary>
		public void IncreaseCounter()
		{
			Counter++;
		}

		/// <summary>
		///     set count of muted messages to 0.
		/// </summary>
		public void ResetCounter()
		{
			Counter = 0;
		}

		public void SetUntil(DateTime dateTime)
		{
			Until = dateTime;
		}
	}
}