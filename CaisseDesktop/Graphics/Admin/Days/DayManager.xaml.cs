using System;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseLibrary.Utils;
using CaisseServer;
using CaisseServer.Events;
using MaterialDesignThemes.Wpf;

namespace CaisseDesktop.Graphics.Admin.Days
{
    /// <summary>
    /// Interaction logic for DayManager.xaml
    /// </summary>
    public partial class DayManager : Window
    {
        private EvenementManager Manager { get; }
        private SaveableDay Day { get; }
        private bool New { get; }
        private bool FirstClose { get; set; }

        public DayManager(EvenementManager manager, SaveableDay day)
        {
            InitializeComponent();
            this.Owner = manager;
            Manager = manager;
            Day = day;
            New = day == null;
            FirstClose = New;

            CombinedCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                manager.Evenement.Start.AddDays(-1)));
            CombinedCalendar.BlackoutDates.Add(new CalendarDateRange(manager.Evenement.End.AddDays(1),
                DateTime.MaxValue));


            EndCombinedCalendar.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                manager.Evenement.Start.AddDays(-1)));
            EndCombinedCalendar.BlackoutDates.Add(new CalendarDateRange(manager.Evenement.End.AddDays(1),
                DateTime.MaxValue));

            if (!New)
            {
                DataContext = new DayPickerModel(day.Start, day.End);
                return;
            }

            DataContext = new DayPickerModel(manager.Evenement.Start, manager.Evenement.Start.AddHours(24));


            Day = new SaveableDay
            {
                Event = manager.Evenement
            };
        }

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
            new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);

        private void SaveDayButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pickerModel = (DayPickerModel) DataContext;

            var test = (long) pickerModel.End.ToUnixTimeStamp() - (long) pickerModel.Start.ToUnixTimeStamp();

            if (test <= 0)
            {
                MessageBox.Show("La fin de l'évenement ne peut pas être avant le début.");
                return;
            }

            if (test > 60 * 60 * 24 &&
                MessageBox.Show("Le jour dure plus de 24h, es-tu sûr de vouloir sauvegarder ?",
                    "Jour supérieur à 24h.", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            if (CombinedCalendar.SelectedDate == null || EndCombinedCalendar.SelectedDate == null)
            {
                MessageBox.Show("Une erreur est survenue, veuillez réessayer", "Une erreur est survenue",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var start = ((DayPickerModel) DataContext).Start;
            var end = ((DayPickerModel) DataContext).End;

            using (var db = new CaisseServerContext())
            {
                var days = db.Days.Where(t => t.Event.Id == Manager.Evenement.Id).ToList();

                foreach (var day in days)
                {
                    // < 0 : date est avant valeur
                    // == 0 : date est égale valeur
                    // > 0 date est après valeur

                    /*
                     *
                     * si value1 est après day1 && day2 est avant value2 || si value1 est avant day1 && day2 est apres value2
                     */

                    if ((start.CompareTo(day.Start) <= 0 || day.End.CompareTo(end) >= 0) &&
                        (start.CompareTo(day.Start) >= 0 || day.End.CompareTo(end) <= 0) || MessageBox.Show(
                            "Le jour chevauche un autre jour déjà enregistré, es-tu sûr de vouloir sauvegarder ?",
                            "Jour chevauche un autre.", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
                    Save(db);
                    return;
                }

                Save(db);
            }

            //MessageBox.Show("Sauvegarde...");
        }

        private void Save(CaisseServerContext db)
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            Debug.Assert(CombinedCalendar.SelectedDate != null, "CombinedCalendar.SelectedDate != null");
            Debug.Assert(EndCombinedCalendar.SelectedDate != null, "EndCombinedCalendar.SelectedDate != null");

            Day.Start = ((DayPickerModel) DataContext).Start;
            Day.End = ((DayPickerModel) DataContext).End;
            db.Events.Attach(Day.Event);

            if (New)
            {
                db.Days.Add(Day);
            }
            else
            {
                //db.Days.Attach(Day);
                db.Days.AddOrUpdate(Day);
            }

            db.SaveChanges();

            Dispatcher.Invoke(() =>
            {
                if (Manager.MasterFrame.ToCustomPage().CustomName.Equals("EventDayPage"))
                {
                    if (New) Manager.MasterFrame.ToCustomPage().Add(Day);
                    else Manager.MasterFrame.ToCustomPage().Update();
                }

                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "Le jour a bien été crée !" : "Le jour a bien été enregistré !");

                Close();
            });
        }
    }
}