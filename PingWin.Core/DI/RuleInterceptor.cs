using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;
using PingWin.Entities.Models;

namespace PingWin.Core
{
	public class TestInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			invocation.Proceed();
		}
	}

	public class RuleInterceptorAttribute : InterceptAttribute
	{
		public override IInterceptor CreateInterceptor(IProxyRequest request)
		{
			return request.Context.Kernel.Get<RuleInterceptor>();
		}
	}

	public class RuleInterceptor : SimpleInterceptor
	{
		readonly Stopwatch _stopwatch = new Stopwatch();

		protected override void BeforeInvoke(IInvocation invocation)
		{
			Trace.WriteLine($"Rule start: {GetWrappedType(invocation)}");

			_stopwatch.Start();
		}

		private static string GetWrappedType(IInvocation invocation)
		{
			string name = invocation.Request.Context.Binding.Service.Name;

			return name;
		}


		private static string GetStatus(IInvocation invocation)
		{
			var log = invocation.ReturnValue as Log;

			return log?.StatusEnum.ToString() ?? "ERROR";
		}

		protected override void AfterInvoke(IInvocation invocation)
		{
			_stopwatch.Stop();

			string totalSeconds = _stopwatch.Elapsed.TotalSeconds.ToString("N2");

			//Trace.WriteLine($"Finish job: {GetJobName(invocation)} ({totalSeconds} s.)");
			Trace.WriteLine($"Rule {GetStatus(invocation)}: {GetWrappedType(invocation)}");

			_stopwatch.Reset();
		}
	}
}