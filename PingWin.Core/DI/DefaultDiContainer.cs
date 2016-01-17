using Ninject;
using Ninject.Syntax;

namespace PingWin.Core
{
	public static class DefaultDiContainer
	{
		static DefaultDiContainer()
		{
			Kernel = new StandardKernel(new RealModule());
		}

		public static IKernel Kernel { get; set; }

		public static T GetService<T>()
		{
			return (T)Kernel.GetService(typeof(T));
		}

	}
}