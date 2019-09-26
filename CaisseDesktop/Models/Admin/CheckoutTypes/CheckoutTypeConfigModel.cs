using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Admin;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Admin.CheckoutTypes;
using CaisseDesktop.Graphics.Admin.CheckoutTypes.Pages;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.IO;
using CaisseDesktop.Lang;
using CaisseDesktop.Utils;
using CaisseLibrary.Enums;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Admin.CheckoutTypes
{
	public class CheckoutTypeConfigModel : INotifyPropertyChanged
	{

		public readonly SaveableCheckoutType CheckoutType;
		public readonly Dispatcher Dispatcher;

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

        private ICommand _backCommand;
		public ICommand BackCommand => _backCommand ?? (_backCommand = new CommandHandler(Back, true));

		public Action CloseAction { get; set; }
		private readonly EventManagerModel ParentModel;

		public CheckoutTypeConfigModel(EventManagerModel parentModel, SaveableCheckoutType checkoutType, Dispatcher dispatcher)
		{
			ParentModel = parentModel;
			IsCreating = checkoutType == null;
			CheckoutType = checkoutType ?? new SaveableCheckoutType { Event = ParentModel.SaveableEvent };
			Dispatcher = dispatcher;
			SwitchPage(IsCreating ? CaisseLibrary.Enums.CheckoutType.Tickets : (CheckoutType)CheckoutType.Type);
			_canSave = IsCreating;
			Task.Run(LoadCheckoutNames);
		}

		public bool IsCreating;

		private bool _canSave;

		public bool CanSave
		{
			get => _canSave;
			set
			{
				_canSave = value;
				OnPropertyChanged();
			}
		}

		public CustomPage ActualPage
		{
			get => _actualPage;
			set
			{
				_actualPage = value;
				OnPropertyChanged();
			}
		}


		public CheckoutType Type
		{
			get => (CheckoutType)CheckoutType.Type;
			set
			{
				CheckoutType.Type = (int)value;
				SwitchPage(value); // switch page
				CanSave = true;
				OnPropertyChanged();
			}
		}

		public string Name
		{
			get => CheckoutType.Name;
			set
			{
				CanSave = true;
				CheckoutType.Name = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<string> _checkoutNameList;
		private CustomPage _actualPage;

		public ObservableCollection<string> CheckoutNameList
		{
			get => _checkoutNameList;
			set
			{
				_checkoutNameList = value;
				OnPropertyChanged();
			}
		}

		private void SwitchPage(CheckoutType type)
		{
			switch (type)
			{
				case CaisseLibrary.Enums.CheckoutType.Tickets:
					ActualPage = new CheckoutTypeArticlePage(this);
					break;
				case CaisseLibrary.Enums.CheckoutType.Food:
					ActualPage = new CheckoutTypeTicketsPage(this);
					break;
				case CaisseLibrary.Enums.CheckoutType.Consign:
					ActualPage = new CheckoutTypeConsignPage();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		private void Back(object arg)
		{
			CloseAction();
		}

		private void Save(object arg)
		{

			if (string.IsNullOrWhiteSpace(Name))
			{
				System.Windows.Forms.MessageBox.Show(French.Exception_ArgsMissing);
				return;
			}

			Task.Run(Save);
		}

		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				if (IsCreating)
				{
					if (db.CheckoutTypes.Any(t =>
						t.Event.Id == ParentModel.SaveableEvent.Id && t.Name.ToLower().Equals(CheckoutType.Name.ToLower())))
					{
						Dispatcher.Invoke(() => { return Mouse.OverrideCursor = null; });

						Validations.ShowWarning("Impossible de créer ce type de caisse. Ce nom est déjà utilisé!");
						return;
					}

					db.Events.Attach(CheckoutType.Event);
					db.CheckoutTypes.Add(CheckoutType);
				}
				else
				{
					db.CheckoutTypes.Attach(CheckoutType);
				}


				db.Entry(CheckoutType).State = IsCreating ? EntityState.Added : EntityState.Modified;
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				CanSave = false;
				Mouse.OverrideCursor = null;
				MessageBox.Show("Le type de caisse a bien été " + (IsCreating ? "crée" : "enregistré") + " !");
			});
		}

		private async void LoadCheckoutNames()
		{
			if (CheckoutType == null) return;

			using (var db = new CaisseServerContext())
			{
				var checkoutNames = await db.Checkouts.Where(t => t.CheckoutType.Id == CheckoutType.Id).Select(t => t.Name).ToListAsync();

				Dispatcher.Invoke(() =>
				{
					CheckoutNameList = new ObservableCollection<string>(checkoutNames);
				});
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}
