using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using CaisseDesktop.Enums;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Graphics.Admin.CheckoutTypes;
using CaisseDesktop.Graphics.Admin.Days;
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseDesktop.Graphics.Admin.Owners;
using CaisseDesktop.Graphics.Admin.PaymentMethods;
using CaisseDesktop.Graphics.Print;
using CaisseDesktop.Models.Admin;
using CaisseDesktop.Utils;
using CaisseLibrary.Data;
using CaisseServer;
using CaisseServer.Events;
using Microsoft.Win32;
using ProtoBuf;

namespace CaisseDesktop.Graphics.Admin.Events
{
	/// <summary>
	///     Interaction logic for EventManager.xaml
	/// </summary>
	public partial class EventManager
	{

		private EventManagerModel Model => DataContext as EventManagerModel;

		public EventManager(SaveableEvent saveableEvent)
		{
			InitializeComponent();
			DataContext = new EventManagerModel(WindowType.Events, saveableEvent);
			Model.CloseAction = Close;
		}

		private void CreateCheckout_OnClick(object sender, RoutedEventArgs e)
		{
			new CheckoutManager(this, null).ShowDialog();
		}

		private void CreateOwner_OnClick(object sender, RoutedEventArgs e)
		{
			new OwnerManager(this, null).ShowDialog();
		}

		private void CreateCheckoutType_OnClick(object sender, RoutedEventArgs e)
		{
			new CheckoutTypeManager(this, null).ShowDialog();
		}

		private void CreateDay_OnClick(object sender, RoutedEventArgs e)
		{
			new DayManager(Model, null).ShowDialog();

		}

		private void CreatePaymentMethod_OnClick(object sender, RoutedEventArgs e)
		{
			new PaymentMethodManager(this, null).ShowDialog();
		}

		private void Export_OnClick(object sender, RoutedEventArgs e)
		{
			if (Model.SaveableEvent == null) return;

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
				saveableEvent.From(Model.SaveableEvent, db);

			using (var file = File.Create(path))
				Serializer.Serialize(file, saveableEvent);

			MessageBox.Show("Le fichier a bien été enregistré !");
		}

	}
}