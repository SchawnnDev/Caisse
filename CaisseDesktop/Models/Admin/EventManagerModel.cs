using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Enums;
using CaisseDesktop.Graphics.Admin;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Graphics.Admin.CheckoutTypes;
using CaisseDesktop.Graphics.Admin.Days;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseDesktop.Graphics.Admin.Owners;
using CaisseDesktop.Graphics.Admin.PaymentMethods;
using CaisseDesktop.Graphics.Utils;
using CaisseLibrary.Data;
using CaisseServer;
using CaisseServer.Events;
using Microsoft.Win32;
using ProtoBuf;
using EventManager = CaisseDesktop.Graphics.Admin.Events.EventManager;

namespace CaisseDesktop.Models.Admin
{
	public class EventManagerModel : INotifyPropertyChanged
	{
		public SaveableEvent SaveableEvent { set; get; }

		private ICommand _browseCommand;
		public ICommand BrowseCommand => _browseCommand ?? (_browseCommand = new CommandHandler(Browse, true));

		private ICommand _backCommand;
		public ICommand BackCommand => _backCommand ?? (_backCommand = new CommandHandler(Back, true));

		private ICommand _createCheckoutCommand;
		public ICommand CreateCheckoutCommand => _createCheckoutCommand ?? (_createCheckoutCommand = new CommandHandler(CreateCheckout, true));

		private ICommand _createCheckoutTypeCommand;
		public ICommand CreateCheckoutTypeCommand => _createCheckoutTypeCommand ?? (_createCheckoutTypeCommand = new CommandHandler(CreateCheckoutType, true));

		private ICommand _createOwnerCommand;
		public ICommand CreateOwnerCommand => _createOwnerCommand ?? (_createOwnerCommand = new CommandHandler(CreateOwner, true));

		private ICommand _createDayCommand;
		public ICommand CreateDayCommand => _createDayCommand ?? (_createDayCommand = new CommandHandler(CreateDay, true));

		private ICommand _createPaymentMethodCommand;
		public ICommand CreatePaymentMethodCommand => _createPaymentMethodCommand ?? (_createPaymentMethodCommand = new CommandHandler(CreatePaymentMethod, true));

		private ICommand _exportCommand;
		public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new CommandHandler(Export, true));

		public Action CloseAction { get; set; }

		private readonly EventManager _parentWindow;

		private CustomPage _actualPage;

		public CustomPage ActualPage
		{
			get => _actualPage;
			set
			{
				_actualPage = value;
				OnPropertyChanged();
			}
		}

		private WindowType _type;

		public WindowType WindowType
		{
			get => _type;
			set
			{
				_type = value;
				OnPropertyChanged();
			}
		}

		public void Back(object arg)
		{
			if (!ActualPage.CanBack()) return;
			new EventBrowser().Show();
			CloseAction();
		}

		public void Browse(object arg)
		{
			if (!(arg is WindowType type)) return;

			WindowType = type;

			switch (type)
			{
				case WindowType.Checkouts:
					ActualPage = new EventCheckoutTypePage(this);
					break;
				case WindowType.CheckoutTypes:
					ActualPage = new EventCheckoutTypePage(this);
					break;
				case WindowType.Days:
					ActualPage = new EventDayPage(this);
					break;
				case WindowType.Events:
					ActualPage = new EventMainPage(this);
					break;
				case WindowType.Owners:
					ActualPage = new EventOwnerPage(this);
					break;
				case WindowType.PaymentMethods:
					ActualPage = new EventPaymentMethodPage(this);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

		}

		private void CreateCheckout(object arg)
		{
			new CheckoutManager(this, null).ShowDialog();
		}

		private void CreateCheckoutType(object arg)
		{
			new CheckoutTypeManager(this, null).ShowDialog();
		}


		private void CreateOwner(object arg)
		{
			new OwnerManager(this, null).ShowDialog();
		}
		private void CreateDay(object arg)
		{
			new DayManager(this, null).ShowDialog();
		}

		private void CreatePaymentMethod(object arg)
		{
			new PaymentMethodManager(this, null).ShowDialog();
		}

		private void Export(object arg)
		{
			if (SaveableEvent == null) return;

			var saveFileDialog = new SaveFileDialog
			{
				Title = "Select a folder",
				CheckPathExists = true,
				Filter = "Caisse Files|*.caisse"
			};

			if (saveFileDialog.ShowDialog() != true) return;

			var path = saveFileDialog.FileName;

			if (!path.EndsWith(".caisse"))
				path += ".caisse";

			var saveableEvent = new Event();

			using (var db = new CaisseServerContext())
				saveableEvent.From(SaveableEvent, db);

			using (var file = File.Create(path))
				Serializer.Serialize(file, saveableEvent);

			MessageBox.Show("Le fichier a bien été enregistré !");
		}


		public EventManagerModel(WindowType type, SaveableEvent saveableEvent)
		{
			WindowType = type;
			SaveableEvent = saveableEvent;
			Browse(WindowType.Events);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
