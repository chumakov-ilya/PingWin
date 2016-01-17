using Ninject;
using Ninject.Modules;
using NUnit.Framework;
using PingWin.Core;

namespace PingWin.SmokeTests
{
	public class HttpRequestRule_Tests
	{
		[Test]
		public void Execute_Test()
		{
			//HttpRequestRule rule = new HttpRequestRule("http://httpbin.org/post");
			//rule.SetMethod("POST");

			//rule.ExecuteAsync().Wait();
		}

		[Test]
		public void ExecuteIso_Test()
		{
			DefaultDiContainer.Kernel = MockDiContainer.Kernel;

			HttpRequestRule rule = HttpRequestRule.Create("http://httpbin.org/post");

			rule.SetMethod("POST");

			//rule.RestFactory = 

			rule.ExecuteAsync().Wait();
		} 
	}

	//public class FuckU : NinjectModule
	//{
	//	public override void Load()
	//	{
	//		BindToMock<ILogRepository>();
	//		BindToMock<IRestFactory>();
	//		BindToMock<IRestClient>();
	//		BindToMock<IRestRequest>();
	//		BindToMock<IRestResponse>();
	//	}
	//}
}