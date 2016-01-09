using System;
using System.Collections.Generic;
using System.Configuration;

namespace PingWin.Core
{
	public class ConfigurationHelper
	{
		public string MailHost => GetAppSettingsParam("Mail.Host");
		public int MailPort => GetAppSettingsParam("Mail.Port", Convert.ToInt32, 25);
		public string MailUserName => GetAppSettingsParam("Mail.UserName");
		public string MailPassword => GetAppSettingsParam("Mail.Password");
		public string MailFrom => GetAppSettingsParam("Mail.From");
		public string MailTo => GetAppSettingsParam("Mail.To");

		public string GetAppSettingsParam(string paramName)
		{
			return ConfigurationManager.AppSettings[paramName];
		}

		public T GetAppSettingsParam<T>(string paramName, Func<string, T> convertFunc, T defaultValue)
		{
			try
			{
				var value = ConfigurationManager.AppSettings[paramName];
				return String.IsNullOrEmpty(value) ? defaultValue : convertFunc(value);
			}
			catch (Exception ex)
			{
				return defaultValue;
			}
		}
	}
}