using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CaisseDesktop.Enums;
using CaisseDesktop.Graphics.Admin;
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Print;
using CaisseDesktop.Graphics.Utils;
using CaisseLibrary.Data;
using CaisseServer;
using Microsoft.Win32;

namespace CaisseDesktop.Models.Admin.Checkouts
{
	public class CheckoutManagerModel : INotifyPropertyChanged
	{
		public SaveableCheckout SaveableCheckout { set; get; }

		private ICommand _browseCommand;
		public ICommand BrowseCommand => _browseCommand ?? (_browseCommand = new CommandHandler(Browse, true));

		private ICommand _backCommand;
		public ICommand BackCommand => _backCommand ?? (_backCommand = new CommandHandler(Back, true));


		private ICommand _reportCommand;
		public ICommand ReportCommand => _reportCommand ?? (_reportCommand = new CommandHandler(Report, true));
		public Action CloseAction { get; set; }

		public readonly EventManagerModel ParentModel;

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

		private CheckoutPageType _type;

		public CheckoutPageType CheckoutPageType
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

		public void Report(object arg)
		{

			var saveFileDialog = new SaveFileDialog
			{
				Title = "Select a folder",
				CheckPathExists = true,
				Filter = "Text Files|*.txt"
			};

			if (saveFileDialog.ShowDialog() != true) return;

			var path = saveFileDialog.FileName;

			new Test(SaveableCheckout, path).Generate();
		}

		public void Browse(object arg)
		{
			if (!(arg is CheckoutPageType type)) return;

			CheckoutPageType = type;

			switch (type)
			{
				case CheckoutPageType.DisplayCashiers:
					ActualPage = new CheckoutCashierPage(this);
					break;
				case CheckoutPageType.DisplayEdt:
					ActualPage = new CheckoutTimeTablePage(this);
					break;
				case CheckoutPageType.EditInfos:
					ActualPage = new CheckoutMainPage(this);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

		}

		public CheckoutManagerModel(EventManagerModel parentModel, CheckoutPageType type, SaveableCheckout saveableCheckout)
		{
			ParentModel = parentModel;
			CheckoutPageType = type;
			SaveableCheckout = saveableCheckout;
			Browse(CheckoutPageType.EditInfos);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
