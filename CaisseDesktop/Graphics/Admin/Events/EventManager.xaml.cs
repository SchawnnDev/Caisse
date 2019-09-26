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

	}
}