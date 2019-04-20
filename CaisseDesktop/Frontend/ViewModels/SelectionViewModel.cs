using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CaisseDesktop.Frontend.ViewModels
{
	public class SelectionViewModel : ReactiveObject
	{

		public ReactiveCommand<Unit, Unit> OpenAdmin { get; private set; }

		public SelectionViewModel()
		{
			OpenAdmin = ReactiveCommand.Create(() => { });
		}


	}
}
