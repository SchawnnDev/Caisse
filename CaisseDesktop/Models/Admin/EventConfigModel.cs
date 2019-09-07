using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CaisseDesktop.Exceptions;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseServer.Events;
using Microsoft.Win32;

namespace CaisseDesktop.Models.Admin
{
    public class EventConfigModel : INotifyPropertyChanged
    {

	    private readonly string _defaultImagePath =
		    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images\logo_brique.jpg");

		public SaveableEvent Event;

	    private ICommand _editImageCommand;
	    public ICommand EditImageCommand => _editImageCommand ?? (_editImageCommand = new CommandHandler(EditImage, true));

		public EventConfigModel(SaveableEvent saveableEvent)
	    {
		    Event = saveableEvent;
	    }

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

	    public string AddressNumber
	    {
		    get => Event.AddressNumber;
		    set
		    {
			    Event.AddressNumber = value;
			    OnPropertyChanged();
		    }
	    }

		public string PostalCode
	    {
		    get => "";
		    set
		    {
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
		    get => Event == null||string.IsNullOrEmpty(Event.ImageSrc) ? _defaultImagePath : Event.ImageSrc;
		    set
		    {
			    Event.ImageSrc = value;
			    OnPropertyChanged();
		    }
	    }

	    public string City
		{
		    get => Event.City;
		    set
		    {
			    Event.City = value;
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

	    public void EditImage(object arg)
	    {
		    var openFileDialog = new OpenFileDialog
		    {
			    Title = "Selectionne une image",
			    InitialDirectory = string.IsNullOrWhiteSpace(ImageSrc)
				    ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
				    : ImageSrc,
			    Filter = "Fichier images|*.jpg;*.jpeg;*.bmp"
		    };

		    if (openFileDialog.ShowDialog() != true) return;

		    ImageSrc = openFileDialog.FileName;
		}

		public event PropertyChangedEventHandler PropertyChanged;

	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }

	}
}
