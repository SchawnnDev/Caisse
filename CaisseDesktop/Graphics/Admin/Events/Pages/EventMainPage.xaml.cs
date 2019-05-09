using System;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;
using MaterialDesignThemes.Wpf;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
	/// <summary>
	///     Interaction logic for MainEventManager.xaml
	/// </summary>
	public partial class EventMainPage
	{
		public EventMainPage(EvenementManager parentWindow)
		{
			InitializeComponent();
			ParentWindow = parentWindow;
			DataContext = new EventMainPageModel(parentWindow.Evenement);
		}

		private bool New { get; } = true;
		private bool Saved { get; set; }
		private bool FirstClose { get; set; } = true;
		private EvenementManager ParentWindow { get; }
		private EventMainPageModel Model => DataContext as EventMainPageModel;
		public override string CustomName => "EventMainPage";

		private void Save_OnClick(object sender, RoutedEventArgs e)
		{
			if (Check(EventName)
				//|| Check(EventStart) || Check(EventEnd) || Check(EventAddresse) 
				|| Check(EventDescription))
				return;

			if (ParentWindow.Evenement == null)
				ParentWindow.Evenement = new SaveableEvent();

			ParentWindow.Evenement.Name = EventName.Text;
			ParentWindow.Evenement.Description = EventDescription.Text;
			//ParentWindow.Evenement.Address = EventAddresse.Text;
			// ParentWindow.Evenement.Start = EventStart.Value.GetValueOrDefault();
			// ParentWindow.Evenement.End = EventEnd.Value.GetValueOrDefault();

			Task.Run(() => Save());
		}

		public void CombinedDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
		{
			if (!(eventArgs.Session.Content is Grid grid)) return;

			if (grid.Name.Equals("StartGrid"))
			{
				CombinedCalendar.SelectedDate = ((DayPickerModel)DataContext).Start;
				CombinedClock.Time = ((DayPickerModel)DataContext).Start;
				return;
			}

			CombinedEndCalendar.SelectedDate = ((DayPickerModel)DataContext).End;
			CombinedEndClock.Time = ((DayPickerModel)DataContext).End;
		}

		public void CombinedDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
		{
			if (!Equals(eventArgs.Parameter, "1")) return;
			if (!(eventArgs.Session.Content is Grid grid)) return;

			var start = grid.Name.Equals("StartGrid");
			var date = start ? CombinedCalendar.SelectedDate : CombinedEndCalendar.SelectedDate;
			if (date == null) return;
			var combined = SetTime(date.Value, start ? CombinedClock.Time : CombinedEndClock.Time);
			//var combined = date.Value.AddSeconds(start
			//   ? CombinedClock.Time.TimeOfDay.TotalSeconds
			//   : EndCombinedClock.Time.TimeOfDay.TotalSeconds);

			if (FirstClose && New) // If new then the start = end date
			{
				((DayPickerModel)DataContext).Start = ((DayPickerModel)DataContext).End = combined;
				FirstClose = false;
				return;
			}

			if (start) ((DayPickerModel)DataContext).Start = combined;
			else ((DayPickerModel)DataContext).End = combined;
		}

		public DateTime SetTime(DateTime date, DateTime time) =>
			new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);

		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				db.Events.AddOrUpdate(ParentWindow.Evenement);
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = null;
				MessageBox.Show(New ? "L'événement a bien été crée !" : "L'événement a bien été enregistré !");
				Model.Enabled = false;
				Saved = true;
			});
		}


		public override void Update()
		{
		}

		public override void Add<T>(T item)
		{
		}

		public override bool CanClose() => Saved || !Saved && Validations.WillClose(true);

		public override bool CanBack() => Saved || Validations.WillClose(true);
	}
}