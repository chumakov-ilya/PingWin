using NUnit.Framework;

namespace PingWin.Core.Tests
{
	public class HttpRequestRule_Tests
	{
		[Test]
		public void Execute_Test()
		{
			HttpRequestRule rule = new HttpRequestRule("http://httpbin.org/post");
			rule.SetMethod("POST");

			rule.Execute().Wait();
		} 
	}
}