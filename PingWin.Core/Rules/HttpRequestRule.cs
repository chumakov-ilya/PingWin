﻿using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using PingWin.Entities;
using PingWin.Entities.Models;
using RestSharp;

namespace PingWin.Core
{
	public class HttpRequestRule : IRule
	{
		public string Url { get; private set; }

		public LogRepository LogRepository { get; set; }

		public HttpRequestRule(string url)
		{
			Url = url;
			LogRepository = new LogRepository();
			Code = 200;
			Method = Method.GET;
		}

		public Method Method { get; set; }

		public int Code { get; set; }

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

		public async Task<Log> Execute()
		{
			try
			{
				//ignore SSL errors
				ServicePointManager.ServerCertificateValidationCallback +=
					(sender, certificate, chain, sslPolicyErrors) => true;

				var client = new RestClient(Url);

				IRestRequest request = new RestRequest(Method);

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
			return $"FAILURE: Inaccessible endpoint {Url}";
		}
	}
}