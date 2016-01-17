using Moq;
using Ninject.Modules;
using Ninject.Syntax;
using PingWin.Core;
using PingWin.Core.Rest;
using PingWin.Entities.Models;

namespace PingWin.IsolatedTests
{
	public class MockModule : NinjectModule
	{

		public MockRepository MockRepository { get; set; }

		public MockModule()
		{
			MockRepository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };

			MockHelper =new MockHelper(MockRepository);
		}

		public MockHelper MockHelper { get; set; }

		public override void Load()
		{
			//rebind external dependencies (db etc.) only

			BindToMock<IRestFactory>(MockHelper.RestFactory());
			BindToMock<IContextFactory>(MockHelper.ContextFactory());
			BindToMock<IMailer>(MockHelper.Mailer());
		}

		public IBindingNamedWithOrOnSyntax<T> BindToMock<T>() where T : class
		{
			T mock = MockRepository.Create<T>().Object;

			return BindToMock(mock);
		}

		public IBindingNamedWithOrOnSyntax<T> BindToMock<T>(T mock) where T : class
		{
			return Rebind<T>().ToConstant(mock).InTransientScope();
		}
	}
}