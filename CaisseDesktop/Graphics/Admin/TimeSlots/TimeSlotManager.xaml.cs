using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.TimeSlots
{
    /// <summary>
    /// Interaction logic for TimeSlotManager.xaml
    /// </summary>
    public partial class TimeSlotManager : Window
    {
	    private SaveableSubstituteTimeSlot SubstituteTimeSlot { get; set; }
        private SaveableTimeSlot TimeSlot { get; set; }
        private DateTime Start { get; }
        private DateTime End { get; }
        private bool New { get; }
	    private bool Starting { get; set; } = true;

        public TimeSlotManager(SaveableTimeSlot timeSlot, SaveableDay day, SaveableCheckout checkout, DateTime start, DateTime end)
        {
            InitializeComponent();
            TimeSlot = timeSlot;
            Start = start;
            End = end;
            New = timeSlot == null;

	        if (New)
	        {
		        TimeSlot = new SaveableTimeSlot
		        {
					Checkout =  checkout,
					Day =  day
		        };
	        }
	        else
	        {

		        using (var db = new CaisseServerContext()) // Charger le substitute timeslot
		        {
			        if (db.SubstituteTimeSlots.Any(t => t.TimeSlot.Id == timeSlot.Id))
				        SubstituteTimeSlot = db.SubstituteTimeSlots.First(t => t.TimeSlot.Id == timeSlot.Id);
		        }

		        Fill();
			}

            

	        Loaded += (sender, args) => { Starting = false; };
        }

        private void TogglePause()
        {
            var pause = TimeSlot.Pause = !TimeSlot.Pause;

            TimeSlotCashier.IsEnabled = !pause;
			
            ToggleSubstitute(!pause);
	        TimeSlotSubstitute.IsEnabled = !pause;
        }

        private void ToggleSubstitute(bool toggle)
		{ 
			CheckSubstituteTimeSlot();
            if (toggle && !SubstituteTimeSlot.Substitute) return;
            TimeSlotSubstituteStart.IsEnabled = toggle;
            TimeSlotSubstituteEnd.IsEnabled = toggle;
            TimeSlotSubstituteCashier.IsEnabled = toggle;
        }

        private void Fill()
        {
            /**
             * Fill cashier slots, substitute slots & check if cashier exists
             */
            if (New)
            {
                TimeSlotStart.SelectedTime = Start;
                TimeSlotEnd.SelectedTime = End;
                return;
            }

            TimeSlotStart.SelectedTime = TimeSlot.Start;
	        TimeSlotStart.Text = TimeSlot.Start.ToString("t");
            TimeSlotEnd.SelectedTime = TimeSlot.End;
	        TimeSlotEnd.Text = TimeSlot.End.ToString("t");

            // set and find cashier

            if (SubstituteTimeSlot == null) return;

            TimeSlotSubstituteStart.SelectedTime = SubstituteTimeSlot.Start;
	        TimeSlotSubstituteStart.Text = SubstituteTimeSlot.Start.ToString("t");
			TimeSlotSubstituteEnd.SelectedTime = SubstituteTimeSlot.End;
	        TimeSlotSubstituteEnd.Text = SubstituteTimeSlot.End.ToString("t");

			TimeSlotSubstituteCashier.Content = $"{SubstituteTimeSlot.}"

			// set and find substitute

		}

		private void TimeSlotCashier_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void TimeSlotStart_OnSelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
	        if (Starting) return;
            TimeSlot.Start = e.NewValue ?? Start;
        }

        private void TimeSlotEnd_OnSelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
	        if (Starting) return;
			TimeSlot.End = e.NewValue ?? End;
        }

        private void TimeSlotPause_OnClick(object sender, RoutedEventArgs e)
        {
            TogglePause();
        }

        private void TimeSlotSubstitute_OnClick(object sender, RoutedEventArgs e)
        {
            CheckSubstituteTimeSlot(); // important
            var substitute = SubstituteTimeSlot.Substitute = !SubstituteTimeSlot.Substitute;
            ToggleSubstitute(substitute);
        }

        private void CheckSubstituteTimeSlot()
        {
            if (SubstituteTimeSlot != null) return;

            SubstituteTimeSlot = new SaveableSubstituteTimeSlot
            {
                TimeSlot = this.TimeSlot
            };

        }
    }
}