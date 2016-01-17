using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Ninject;
using Ninject.Syntax;
using PingWin.Core.Rest;
using PingWin.Entities;
using PingWin.Entities.Models;
using RestSharp;

namespace PingWin.SmokeTests
{
	public class MockDiContainer
	{
		public static IKernel Kernel { get; set; }
		public static MockRepository MockRepository { get; set; }

		static MockDiContainer()
		{
			Kernel = new StandardKernel();

			MockRepository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			RegisterAll();
		}

		public static IBindingNamedWithOrOnSyntax<T> BindToMock<T>() where T : class
		{
			T mock = MockRepository.Create<T>().Object;

			return BindToMock(mock);
		}

		public static IBindingNamedWithOrOnSyntax<T> BindToMock<T>(T mock) where T : class
		{
			return Kernel.Rebind<T>().ToConstant(mock).InTransientScope();
		}

		internal static IBindingNamedWithOrOnSyntax<TTo> BindToType<TFrom, TTo>() where TTo : TFrom
		{
			return Kernel.Bind<TFrom>().To<TTo>().InTransientScope();
		}

		public static T GetService<T>()
		{
			return (T)Kernel.GetService(typeof(T));
		}

		public static void RegisterAll()
		{
			//BindToMock<ILogRepository>();
			//BindToMock<ISystemLogRepository>();
			BindToType<ILogRepository, LogRepository>();
			BindToType<ISystemLogRepository, SystemLogRepository>();
			//BindToMock<IRestFactory>();
			////BindToMock<IRestClient>();
			//BindToMock<IRestRequest>();
			//BindToMock<IRestResponse>();

			//BindToMock<IRestClient>(CreateStubRestClient());
			BindToMock<IRestFactory>(CreateStubRestFactory());

			//BindToMock<IContextFactory>();
			BindToMock<IContextFactory>(CreateStubContextFactory());
			//BindToMock<IPingWinContext>(CreateStubContext());

			//BindToMock<IContextFactory>(CreateStubContextFactory());
			//BindToMock<IPingWinContext>(CreateStubContext());
		}

		private static IPingWinContext CreateStubContext()
		{
			var mock = MockRepository.Create<IPingWinContext>();

			mock.Setup(x => x.SaveChanges())
				.Returns(int.MaxValue);

			return mock.Object;
		}

		private static IContextFactory CreateStubContextFactory()
		{
			var mock = MockRepository.Create<IContextFactory>();

			mock.Setup(x => x.Create())
				.Returns(CreateStubContext());

			return mock.Object;
		}

		private static IRestFactory CreateStubRestFactory()
		{
			var mock = MockRepository.Create<IRestFactory>();

			mock.Setup(x => x.CreateClient(It.IsAny<string>()))
				.Returns(CreateStubRestClient());

			return mock.Object;
		}

		public static IRestClient CreateStubRestClient()
		{
			var mock = MockRepository.Create<IRestClient>();

			IRestResponse response = new RestResponse();
			response.StatusCode = (HttpStatusCode)200;

			mock.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>()))
				.Returns(Task.FromResult(response));

			return mock.Object;
		}

	}
}