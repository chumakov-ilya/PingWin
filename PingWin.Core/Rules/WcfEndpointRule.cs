using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace PingWin.Core
{
	public class WcfEndpointRule : IRule
	{
		public string ServiceReference { get; private set; }

		public WcfEndpointRule(string serviceReference)
		{
			ServiceReference = serviceReference;
		}


		public async Task<bool> Execute()
		{
			Trace.WriteLine($"WebTester.Check START");

			//ignore SSL errors
			ServicePointManager.ServerCertificateValidationCallback += 
				(sender, certificate, chain, sslPolicyErrors) => true;

			var client = new RestClient(ServiceReference);

			IRestRequest request = new RestRequest();

			var response = await client.ExecuteTaskAsync(request);

			bool ok = response.StatusCode == HttpStatusCode.OK;

			Trace.WriteLine($"{ok}: {ServiceReference}");

			return ok;
		}

		public string FailureDescription()
		{
			return $"FAILURE: Inaccessible endpoint {ServiceReference}";
		}
	}
}