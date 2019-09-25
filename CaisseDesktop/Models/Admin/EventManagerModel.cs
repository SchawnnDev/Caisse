using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CaisseDesktop.Enums;
using CaisseDesktop.Graphics.Admin;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseDesktop.Graphics.Utils;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Admin
{
	public class EventManagerModel : INotifyPropertyChanged
	{
		public SaveableEvent SaveableEvent { set; get; }

		private ICommand _browseCommand;
		public ICommand BrowseCommand => _browseCommand ?? (_browseCommand = new CommandHandler(Browse, true));

		private ICommand _backCommand;
		public ICommand BackCommand => _backCommand ?? (_backCommand = new CommandHandler(Back, true));

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
