using System.Linq.Expressions;
using RestSharp;

namespace PingWin.Core.Rest
{
	public class RestFactory : IRestFactory
	{
		public IRestClient CreateClient(string url)
		{
			return new RestClient(url);
		} 

		public IRestRequest CreateRequest(Method method)
		{
			return new RestRequest(method);
		} 
	}
}