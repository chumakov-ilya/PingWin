using System.Diagnostics;
using Moq;
using Ninject.Extensions.Interception.Advice.Syntax;
using Ninject.Extensions.Interception.Infrastructure.Language;
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

			BindToMock<IRestFactory>(MockHelper.StubRestFactory());
			BindToMock<IContextFactory>(MockHelper.StubContextFactory());

			BindToMock<IMailer>(MockHelper.StubMailer());

			Rebind<JobRunner>().ToSelf();

			//Rebind<DbConnectionRule>().ToSelf();
			//Rebind<HttpRequestRule>().ToSelf();

			//TODO: not initialize nested properties, why?
			//Rebind<DbConnectionRule>().ToSelf().Intercept().With<RuleInterceptor>();
			//Rebind<HttpRequestRule>().ToSelf().Intercept().With<TestInterceptor>();
		}

		public IBindingNamedWithOrOnSyntax<T> BindToMock<T>(T mock) where T : class
		{
			return Rebind<T>().ToConstant(mock).InTransientScope();
		}

		public IBindingNamedWithOrOnSyntax<T> BindToMock<T>() where T : class
		{
			T mock = MockRepository.Create<T>().Object;

			return BindToMock(mock);
		}
	}
}