using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace PingWin.Core
{
	public class JobInterceptorAttribute : InterceptAttribute
	{
		public override IInterceptor CreateInterceptor(IProxyRequest request)
		{
			return request.Context.Kernel.Get<JobInterceptor>();
		}
	}

	public class JobInterceptor : SimpleInterceptor
	{
		readonly Stopwatch _stopwatch = new Stopwatch();

		protected override void BeforeInvoke(IInvocation invocation)
		{
			Trace.WriteLine("Start job: " + GetJobName(invocation));

			_stopwatch.Start();
		}

		private static string GetJobName(IInvocation invocation)
		{
			IJob job = invocation.Request.Arguments.First() as IJob;

			return job.Name;
		}

		protected override void AfterInvoke(IInvocation invocation)
		{
			_stopwatch.Stop();

			Trace.WriteLine("Finish job: " + GetJobName(invocation));


			//string message = $"[Execution of {invocation.Request.Method} took {_stopwatch.Elapsed}.]";

			//Trace.WriteLine(message);

			_stopwatch.Reset();
		}
	}
}