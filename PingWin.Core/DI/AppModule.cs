using System.Diagnostics;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Modules;
using PingWin.Core.Rest;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class AppModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISystemLogRepository>().To<SystemLogRepository>();
			Bind<ILogRepository>().To<LogRepository>();
			//Bind<JobRepository>().ToSelf();
			//Bind<ReportRepository>().ToSelf();


			Bind<IRestFactory>().To<RestFactory>();
			Bind<IContextFactory>().To<ContextFactory>();

			Bind<IMailer>().To<Mailer>();
		}
	}
}