using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PingWin.Core;
using PingWin.Core.Rest;
using PingWin.Entities.Models;
using RestSharp;

namespace PingWin.IsolatedTests
{
	public class MockHelper
	{
		public MockRepository MockRepository { get; set; }

		public MockHelper(MockRepository mockRepository)
		{
			MockRepository = mockRepository;
		}

		private IPingWinContext StubContext()
		{
			var mock = MockRepository.Create<IPingWinContext>();

			mock.Setup(x => x.SaveChanges())
				.Returns(int.MaxValue);

			var logs = StubDbSet<Log>();
			mock.Setup(m => m.Logs).Returns(logs.Object);

			var slogs = new Mock<DbSet<SystemLog>>();
			mock.Setup(m => m.SystemLogs).Returns(slogs.Object);

			return mock.Object;
		}

		private static Mock<DbSet<T>> StubDbSet<T>() where T : class
		{
			var data = new List<T>().AsQueryable();

			var mock = new Mock<DbSet<T>>();

			mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
			mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
			mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			return mock;
		}

		public IContextFactory StubContextFactory()
		{
			var mock = MockRepository.Create<IContextFactory>();

			mock.Setup(x => x.Create())
				.Returns(StubContext());

			return mock.Object;
		}

		public IRestFactory StubRestFactory()
		{
			var mock = MockRepository.Create<IRestFactory>();

			mock.Setup(x => x.CreateClient(It.IsAny<string>()))
				.Returns(StubRestClient());

			return mock.Object;
		}

		public IRestClient StubRestClient()
		{
			var mock = MockRepository.Create<IRestClient>();

			IRestResponse response = new RestResponse();
			response.StatusCode = (HttpStatusCode)200;

			mock.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>()))
				.Returns(Task.FromResult(response));

			return mock.Object;
		}

		public IMailer StubMailer()
		{
			var mock = MockRepository.Create<IMailer>();
			mock.Setup(x => x.SendMailAsync(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			return mock.Object;
		}

		public IConnectionFactory StubConnectionFactory()
		{
			var mock = MockRepository.Create<IConnectionFactory>();

			mock.Setup(x => x.Create(It.IsAny<string>()))
				.Returns(StubDbConnection());

			return mock.Object;
		}

		private DbConnection StubDbConnection()
		{
			var mock = MockRepository.Create<DbConnection>();

			mock.Setup(x => x.OpenAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask);

			return mock.Object;
		}
	}
}