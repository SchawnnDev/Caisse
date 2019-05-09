using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Events;

namespace CaisseDesktop.Models
{
	public class EventMainPageModel : INotifyPropertyChanged
	{

		private SaveableEvent Event { get; }

		public EventMainPageModel(SaveableEvent saveableEvent)
		{
			if (saveableEvent != null)
			{
				Event = saveableEvent;
				_enabled = false;
				return;
			}

			Event = new SaveableEvent { Start = DateTime.Now.AddDays(-1), End = DateTime.Now };
			_enabled = true;
		}

		public bool Enabled
		{
			get => _enabled;
			set
			{
				_enabled = value;
				OnPropertyChanged();
			}
		}

		private bool _enabled;

		public DateTime Start
		{
			get => Event.Start;
			set
			{
				Event.Start = value;
				OnPropertyChanged();
			}
		}

		public DateTime End
		{
			get => Event.End;
			set
			{
				Event.End = value;
				OnPropertyChanged();
			}
		}

		public string OwnerName
		{
			get => Event.AddressName;
			set
			{
				Event.AddressName = value;
				OnPropertyChanged();
			}
		}


		public string OwnerAddress
		{
			get => Event.Address;
			set
			{
				Event.Address = value;
				OnPropertyChanged();
			}
		}


		public string OwnerPostalCodeCity
		{
			get => Event.PostalCodeCity;
			set
			{
				Event.PostalCodeCity = value;
				OnPropertyChanged();
			}
		}


		public string OwnerSiret
		{
			get => Event.Siret;
			set
			{
				Event.Siret = value;
				OnPropertyChanged();
			}
		}


		public string OwnerTelephone
		{
			get => Event.Telephone;
			set
			{
				Event.Telephone = value;
				OnPropertyChanged();
			}
		}


		public string Name
		{
			get => Event.Name;
			set
			{
				Event.Name = value;
				OnPropertyChanged();
			}
		}

		public string Description
		{
			get => Event.Description;
			set
			{
				Event.Description = value;
				OnPropertyChanged();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
