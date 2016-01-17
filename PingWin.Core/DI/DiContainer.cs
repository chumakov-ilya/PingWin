using Ninject;
using Ninject.Syntax;

namespace PingWin.Core
{
	public static class DiContainer
	{
		static DiContainer()
		{
			Kernel = new StandardKernel(new AppModule());
		}

		public static IKernel Kernel { get; set; }

		public static T GetService<T>()
		{
			return (T)Kernel.GetService(typeof(T));
		}

	}
}