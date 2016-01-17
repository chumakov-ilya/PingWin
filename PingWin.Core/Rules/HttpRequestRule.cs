using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Ninject;
using PingWin.Core.Rest;
using PingWin.Entities;
using PingWin.Entities.Models;
using RestSharp;

namespace PingWin.Core
{
	public class HttpRequestRule : IRule
	{
		[Obsolete("Use Create method instead")]
		public HttpRequestRule() {}

		public string Url { get; private set; }

		[Inject]
		public ILogRepository LogRepository { get; set; }

		[Inject]
		public IRestFactory RestFactory { get; set; }

		public static HttpRequestRule Create(string url)
		{
			var instance = DiContainer.GetService<HttpRequestRule>();

			instance.Url = url;

			return instance;
		}

		public Method Method { get; set; } = Method.GET;

		public int Code { get; set; } = 200;

		/// <summary>
		/// Default: 200
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public HttpRequestRule ExpectCode(int code)
		{
			Code = code;
			return this;
		}

		/// <summary>
		/// Default: GET
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public HttpRequestRule SetMethod(string method)
		{
			Method result;

			bool success = Enum.TryParse(method.ToUpper(), out result);

			if (!success) throw new ArgumentException(nameof(method));

			Method = result;

			return this;
		}

		//[RuleInterceptor]
		public virtual async Task<Log> ExecuteAsync()
		{
			//ignore SSL errors
			ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

			IRestClient client = RestFactory.CreateClient(Url);

			IRestRequest request = RestFactory.CreateRequest(Method);

			IRestResponse response = await client.ExecuteTaskAsync(request);

			if ((int)response.StatusCode == Code)
			{
				return LogRepository.CreateLog(StatusEnum.Success);
			}
			else
			{
				var log = LogRepository.CreateLog(StatusEnum.Failure);

				log.Message = FailureDescription();
				log.FullData = $"Actual HTTP code: {(int)response.StatusCode} {response.StatusCode}. Expected: {Code}.";

				return log;
			}
		}

		public string FailureDescription()
		{
			return $"Problem with URL: {Url}";
		}
	}
}