using System.Net;
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

		private IPingWinContext CreateStubContext()
		{
			var mock = MockRepository.Create<IPingWinContext>();

			mock.Setup(x => x.SaveChanges())
				.Returns(int.MaxValue);

			return mock.Object;
		}

		public IContextFactory ContextFactory()
		{
			var mock = MockRepository.Create<IContextFactory>();

			mock.Setup(x => x.Create())
				.Returns(CreateStubContext());

			return mock.Object;
		}

		public IRestFactory RestFactory()
		{
			var mock = MockRepository.Create<IRestFactory>();

			mock.Setup(x => x.CreateClient(It.IsAny<string>()))
				.Returns(CreateStubRestClient());

			return mock.Object;
		}

		public IRestClient CreateStubRestClient()
		{
			var mock = MockRepository.Create<IRestClient>();

			IRestResponse response = new RestResponse();
			response.StatusCode = (HttpStatusCode)200;

			mock.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>()))
				.Returns(Task.FromResult(response));

			return mock.Object;
		}

		public IMailer Mailer()
		{
			var mock = MockRepository.Create<IMailer>();
			mock.Setup(x => x.SendMailAsync(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			return mock.Object;
		}
	}
}