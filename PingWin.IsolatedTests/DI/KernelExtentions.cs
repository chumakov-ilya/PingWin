using Ninject;
using Ninject.Modules;

namespace PingWin.IsolatedTests
{
	public static class KernelExtentions
	{
		public static void Load(this IKernel kernel, INinjectModule module)
		{
			kernel.Load(new[] { module });
		} 
	}
}