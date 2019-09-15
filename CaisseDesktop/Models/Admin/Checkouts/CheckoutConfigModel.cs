using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Admin.Checkouts
{
	public class CheckoutConfigModel : INotifyPropertyChanged
	{

		private SaveableCheckout Checkout;
		public Dispatcher Dispatcher { get; set; }

		public CheckoutConfigModel(SaveableCheckout checkout, bool creating)
		{
			Checkout = checkout;
			CanSave = creating;
		}

		private bool _canSave;
		private ObservableCollection<SaveableCheckoutType> _types;
		private ObservableCollection<SaveableOwner> _owners;

		public bool CanSave
		{
			get => _canSave;
			set
			{
				_canSave = value;
				OnPropertyChanged();
			}
		}

		public string Name
		{
			get => Checkout.Name;
			set
			{
				Checkout.Name = value;
				OnPropertyChanged();
			}
		}

		public string Details
		{
			get => Checkout.Details;
			set
			{
				Checkout.Details = value;
				OnPropertyChanged();
			}
		}

		public SaveableOwner Owner
		{
			get => Checkout.Owner;
			set
			{
				Checkout.Owner = value;
				OnPropertyChanged();
			}
		}

		public SaveableCheckoutType CheckoutType
		{
			get => Checkout.CheckoutType;
			set
			{
				Checkout.CheckoutType = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<SaveableCheckoutType> Types
		{
			get => _types;
			set
			{
				_types = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<SaveableOwner> Owners
		{
			get => _owners;
			set
			{
				_owners = value;
				OnPropertyChanged();
			}
		}

		private void LoadInfos(SaveableEvent saveableEvent)
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			ObservableCollection<SaveableCheckoutType> types;
			ObservableCollection<SaveableOwner> owners;

			using (var db = new CaisseServerContext())
			{
				types = new ObservableCollection<SaveableCheckoutType>(db.CheckoutTypes
					.OrderBy(e => e.Event.Id == saveableEvent.Id).ToList());
				owners = new ObservableCollection<SaveableOwner>(db.Owners
					.Where(t => t.Event.Id == saveableEvent.Id)
					.OrderBy(e => e.LastLogin).ToList());
			}

			Dispatcher.Invoke(() =>
			{
				Types = types;
				Owners = owners;
			//	CheckoutType.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = Types });
		//		CheckoutOwner.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = Owners });

			//	if (!New)
		//		{
	//				CheckoutType.SelectedIndex = Types.FindIndex(t => t.Id == ParentWindow.Checkout.CheckoutType.Id);
	//				CheckoutOwner.SelectedIndex = Owners.FindIndex(t => t.Id == ParentWindow.Checkout.Owner.Id);
	//			}

				Mouse.OverrideCursor = null;
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}
