using System.Windows;
using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Models;
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

        public DayManager(EvenementManager manager, SaveableDay day)
        {
            InitializeComponent();
            this.Owner = manager;
            Manager = manager;
            Day = day;
            New = day == null;

            if (New)
            {
                Day = new SaveableDay();

            }

        }

        public void CalendarDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
//            Calendar.SelectedDate = ((DayPickerModel)DataContext).Date;
        }

        public void CalendarDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, "1")) return;
            /*
            if (!Calendar.SelectedDate.HasValue)
            {
                eventArgs.Cancel();
                return;
            } */

           // ((DayPickerModel)DataContext).Date = Calendar.SelectedDate.Value;
        }

    }
}
