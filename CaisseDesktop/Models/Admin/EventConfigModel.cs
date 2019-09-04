using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Admin
{
    public class EventConfigModel : INotifyPropertyChanged
    {

	    public SaveableEvent Event;

	    public EventConfigModel(SaveableEvent saveableEvent)
	    {
		    Event = saveableEvent;
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

	    public string AddressName
	    {
		    get => Event.AddressName;
		    set
		    {
			    Event.AddressName = value;
			    OnPropertyChanged();
		    }
	    }

	    public string Address
	    {
		    get => Event.Address;
		    set
		    {
			    Event.Address = value;
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

	    public string ImageSrc
	    {
		    get => Event.ImageSrc;
		    set
		    {
			    Event.ImageSrc = value;
			    OnPropertyChanged();
		    }
	    }

	    public string PostalCodeCity
		{
		    get => Event.PostalCodeCity;
		    set
		    {
			    Event.PostalCodeCity = value;
			    OnPropertyChanged();
		    }
	    }

	    public string Siret
	    {
		    get => Event.Siret;
		    set
		    {
			    Event.Siret = value;
			    OnPropertyChanged();
		    }
	    }

	    public string Telephone
	    {
		    get => Event.Telephone;
		    set
		    {
			    Event.Telephone = value;
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
