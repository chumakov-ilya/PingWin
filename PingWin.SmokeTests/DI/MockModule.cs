using Moq;
using Ninject;
using Ninject.Modules;
using Ninject.Syntax;
using PingWin.Core.Rest;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.SmokeTests
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
			BindToMock<IRestFactory>(MockHelper.RestFactory());

			BindToMock<IContextFactory>(MockHelper.ContextFactory());
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