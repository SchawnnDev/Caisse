using System;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Admin;
using CaisseDesktop.Models.Admin.Days;
using CaisseDesktop.Utils;
using CaisseLibrary.Utils;
using CaisseServer;
using CaisseServer.Events;
using MaterialDesignThemes.Wpf;
using EventManager = CaisseDesktop.Graphics.Admin.Events.EventManager;

namespace CaisseDesktop.Graphics.Admin.Days
{
    /// <summary>
    /// Interaction logic for DayManager.xaml
    /// </summary>
    public partial class DayManager
    {

		public DayConfigModel Model => DataContext as DayConfigModel;

        public DayManager(EventManagerModel model, SaveableDay day)
        {
            InitializeComponent();
            //this.Owner = manager;
            DataContext = new DayConfigModel(model, day);
            Model.Dispatcher = Dispatcher;
            Model.CloseAction = Close;

        }
		/*
        public void CombinedDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            if (!(eventArgs.Session.Content is Grid grid)) return;

            if (grid.Name.Equals("StartGrid"))
            {
                CombinedCalendar.SelectedDate = ((DayPickerModel) DataContext).Start;
                CombinedClock.Time = ((DayPickerModel) DataContext).Start;
                return;
            }

            EndCombinedCalendar.SelectedDate = ((DayPickerModel) DataContext).End;
            EndCombinedClock.Time = ((DayPickerModel) DataContext).End;
        }

        public void CombinedDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, "1")) return;
            if (!(eventArgs.Session.Content is Grid grid)) return;

            var start = grid.Name.Equals("StartGrid");
            var date = start ? CombinedCalendar.SelectedDate : EndCombinedCalendar.SelectedDate;
            if (date == null) return;
            var combined = SetTime(date.Value, start ? CombinedClock.Time : EndCombinedClock.Time);
            //var combined = date.Value.AddSeconds(start
            //   ? CombinedClock.Time.TimeOfDay.TotalSeconds
            //   : EndCombinedClock.Time.TimeOfDay.TotalSeconds);

            if (FirstClose && New) // If new then the start = end date
            {
                ((DayPickerModel) DataContext).Start = ((DayPickerModel) DataContext).End = combined;
                FirstClose = false;
                return;
            }

            if (start) ((DayPickerModel) DataContext).Start = combined;
            else ((DayPickerModel) DataContext).End = combined;
        }

        public DateTime SetTime(DateTime date, DateTime time) =>
            new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second); */

        
    }
}