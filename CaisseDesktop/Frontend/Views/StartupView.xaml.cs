using System.Reactive;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CaisseDesktop.Frontend.ViewModels;
using CaisseLibrary;
using CaisseServer;
using ReactiveUI;

namespace CaisseDesktop.Frontend.Views
{
	public partial class StartupView : ReactiveWindow<StartupViewModel>
	{
		public StartupView(StartupViewModel startupViewModel)
		{
			InitializeComponent();
			ViewModel = startupViewModel;
			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel, vm => vm.StatusText, v => v.StatusText.Text).DisposeWith(disposables);

				ViewModel.CloseInteraction.RegisterHandler(interaction =>
				{
					DialogResult = interaction.Input;
					interaction.SetOutput(Unit.Default);
					Close();
				}).DisposeWith(disposables);
			});
		}
	}
}