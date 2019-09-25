using System;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Models.Admin;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
	/// <summary>
	///     Interaction logic for MainEventManager.xaml
	/// </summary>
	public partial class EventMainPage
	{
		public EventMainPage(EventManagerModel parentModel)
		{
			InitializeComponent();
			var parentEvent = parentModel.SaveableEvent ?? new SaveableEvent
			{ Start = DateTime.Now, End = DateTime.Now.AddDays(1) };
			DataContext = new EventConfigModel(parentEvent, parentModel.SaveableEvent == null);
			Model.Dispatcher = Dispatcher;
		}

		private EventConfigModel Model => DataContext as EventConfigModel;

		public override string CustomName => "EventMainPage";

		public override void Update()
		{
		}

		public override void Add<T>(T item)
		{
		}

		public override bool CanClose()
		{
			return !Model.CanSave || Model.CanSave && Validations.WillClose(true);
		}

		public override bool CanBack()
		{
			return !Model.CanSave || Validations.WillClose(true);
		}
	}
}