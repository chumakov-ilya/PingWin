using RestSharp;

namespace PingWin.Core.Rest
{
	public interface IRestFactory
	{
		IRestClient CreateClient(string url);
		IRestRequest CreateRequest(Method method);
	}
}