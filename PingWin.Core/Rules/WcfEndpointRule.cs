using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using PingWin.Entities;
using PingWin.Entities.Models;
using RestSharp;

namespace PingWin.Core
{
	public class WcfEndpointRule : IRule
	{
		public string ServiceReference { get; private set; }

		public LogRepository LogRepository { get; set; }

		public WcfEndpointRule(string serviceReference)
		{
			ServiceReference = serviceReference;
			LogRepository = new LogRepository();
		}

		public async Task<Log> Execute()
		{
			try
			{
				//ignore SSL errors
				ServicePointManager.ServerCertificateValidationCallback +=
					(sender, certificate, chain, sslPolicyErrors) => true;

				var client = new RestClient(ServiceReference);

				IRestRequest request = new RestRequest();

				var response = await client.ExecuteTaskAsync(request);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					return LogRepository.CreateLog(StatusEnum.Success);
				}
				else
				{
					var log = LogRepository.CreateLog(StatusEnum.Failure);
					log.ShortData = $"HTTP StatusCode: {response.StatusCode}";
					log.FullData = response.ToString();
					return log;
				}
			}
			catch (Exception exception)
			{
				return LogRepository.CreateLog(StatusEnum.InternalError, exception);
			}
		}

		public string FailureDescription()
		{
			return $"FAILURE: Inaccessible endpoint {ServiceReference}";
		}
	}
}