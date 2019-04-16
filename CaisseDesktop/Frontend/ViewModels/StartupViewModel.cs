using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using CaisseLibrary;
using CaisseServer;
using ReactiveUI;

namespace CaisseDesktop.Frontend.ViewModels
{
	public class StartupViewModel : ReactiveObject
	{
		private string statusText;
		public string StatusText
		{
			get => statusText;
			private set => this.RaiseAndSetIfChanged(ref statusText, value);
		}

		public ReactiveCommand<Unit, Unit> Start { get; }

		public Interaction<bool?, Unit> CloseInteraction { get; } = new Interaction<bool?, Unit>();

		IObservable<Unit> StartImpl()
		{
			return Observable.StartAsync(async () =>
				{
					using (var db = new CaisseServerContext())
					{
						StatusText = "Création de la base de données...";
						db.Database.CreateIfNotExists();
					}

					StatusText = "Chargement de la librairie...";

					Main.Start();

					// Start the application

					StatusText = "Démarrage de l'application...";

					await Task.Delay(TimeSpan.FromSeconds(2));
				}, RxApp.TaskpoolScheduler)
				.SelectMany(CloseInteraction.Handle(null));
		}

		public StartupViewModel()
		{
			Start = ReactiveCommand.CreateFromObservable(StartImpl);
		}

	}
}
