using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using PingWin.Entities.Models;

namespace PingWin.Core.Tests
{
	public class Entity_Tests
	{
		[Test]
		public void JobQuery()
		{
			using (var context = new PingWinContext())
			{
				List<Entities.Models.JobRecord> jobs = context.JobRecords.ToList();

				Assert.IsNotEmpty(jobs);
			}
		}
	}
}