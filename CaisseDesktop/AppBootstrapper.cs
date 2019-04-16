using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CaisseDesktop.Frontend.ViewModels;
using ReactiveUI;
using Splat;

namespace CaisseDesktop
{
	public class AppBootstrapper
	{
		public AppBootstrapper()
		{
			Locator.CurrentMutable.Register(() => new StartupViewModel());
			Locator.CurrentMutable.Register(() => new SelectionViewModel());

			Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
		}
	}
}
