using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Exceptions;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseServer;
using CaisseServer.Events;
using Cursors = System.Windows.Input.Cursors;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace CaisseDesktop.Models.Admin
{
	public class EventConfigModel : INotifyPropertyChanged
	{

		private readonly string _defaultImagePath =
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images\logo_brique.jpg");

		public SaveableEvent Event;

		public Dispatcher Dispatcher;

		private ICommand _editImageCommand;
		public ICommand EditImageCommand => _editImageCommand ?? (_editImageCommand = new CommandHandler(EditImage, true));

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		public EventConfigModel(SaveableEvent saveableEvent, bool creating)
		{
			Event = saveableEvent;
			IsCreating = creating;
			_canSave = IsCreating;
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

		public DateTime Start
		{
			get => Event.Start;
			set
			{
				CanSave = true;
				Event.Start = value;
				OnPropertyChanged();
			}
		}

		public DateTime End
		{
			get => Event.End;
			set
			{
				CanSave = true;
				Event.End = value;
				OnPropertyChanged();
			}
		}

		public string Name
		{
			get => Event.Name;
			set
			{
				CanSave = true;
				Event.Name = value;
				OnPropertyChanged();
			}
		}

		public string AddressName
		{
			get => Event.AddressName;
			set
			{
				CanSave = true;
				Event.AddressName = value;
				OnPropertyChanged();
			}
		}

		public string Address
		{
			get => Event.Address;
			set
			{
				CanSave = true;
				Event.Address = value;
				OnPropertyChanged();
			}
		}

		public string AddressNumber
		{
			get => Event.AddressNumber;
			set
			{
				CanSave = true;
				Event.AddressNumber = value;
				OnPropertyChanged();
			}
		}

		public string PostalCode
		{
			get => Event.PostalCode;
			set
			{
				CanSave = true;
				Event.PostalCode = value;
				OnPropertyChanged();
			}
		}

		public string ImageSrc
		{
			get => Event == null || string.IsNullOrEmpty(Event.ImageSrc) ? _defaultImagePath : Event.ImageSrc;
			set
			{
				CanSave = true;
				Event.ImageSrc = value;
				OnPropertyChanged();
			}
		}

		public string City
		{
			get => Event.City;
			set
			{
				CanSave = true;
				Event.City = value;
				OnPropertyChanged();
			}
		}

		public string Siret
		{
			get => Event.Siret;
			set
			{
				CanSave = true;
				Event.Siret = value;
				OnPropertyChanged();
			}
		}

		public string Telephone
		{
			get => Event.Telephone;
			set
			{
				CanSave = true;
				Event.Telephone = value;
				OnPropertyChanged();
			}
		}

		private void AddOrUpdate()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				db.Events.AddOrUpdate(Event);
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				CanSave = false;
				Mouse.OverrideCursor = null;
				MessageBox.Show(IsCreating ? French.Event_Created : French.Event_Saved);
			});
		}

		public void Save(object arg)
		{

			if (string.IsNullOrWhiteSpace(Name))
			{
				MessageBox.Show(French.Exception_ArgsMissing);
				return;
			}

			Task.Run(() => AddOrUpdate());

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
