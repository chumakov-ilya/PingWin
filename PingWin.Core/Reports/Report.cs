using System;
using System.Threading.Tasks;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class Report
	{
		public Report()
		{
			RunInterval = TimeSpan.FromMinutes(10);
		}

		public async Task ExecuteAsync()
		{
			await Task.Run(() => Execute());
		}

		private void Execute()
		{
		}

		public TimeSpan RunInterval { get; }
	}
}