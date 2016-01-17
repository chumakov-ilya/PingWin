using Ninject.Modules;
using PingWin.Core.Rest;
using PingWin.Entities;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class RealModule : NinjectModule
	{
		public override void Load()
		{
			this.Bind<ISystemLogRepository>().To<SystemLogRepository>();
			this.Bind<ILogRepository>().To<LogRepository>();
			this.Bind<IRestFactory>().To<RestFactory>();
			this.Bind<IContextFactory>().To<ContextFactory>();
			this.Bind<JobRunner>().ToSelf();
			this.Bind<JobRoot>().ToSelf();
			this.Bind<JobRepository>().ToSelf();

			this.Bind<ReportRunner>().ToSelf();
			this.Bind<ReportRoot>().ToSelf();
		}
	}
}