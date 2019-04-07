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
		private SaveableCashier Cashier { get; set; }
		private SaveableSubstitute Substitute { get; set; }
		private SaveableTimeSlot TimeSlot { get; set; }
		private readonly DateTime Start;
		private readonly DateTime End;
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
					Checkout = checkout,
					Day = day
				};
			}
			else
			{

				using (var db = new CaisseServerContext()) // Charger le substitute timeslot
				{
					if (db.Substitutes.Any(t => t.TimeSlot.Id == timeSlot.Id))
						Substitute = db.Substitutes.First(t => t.TimeSlot.Id == timeSlot.Id);

					if (db.Cashiers.Any(t => t.TimeSlot.Id == timeSlot.Id))
						Cashier = db.Cashiers.First(t => t.TimeSlot.Id == timeSlot.Id);

					var cashiers = db.Cashiers.Where(t => t.TimeSlot.Checkout.Id == timeSlot.Checkout.Id).ToList();

					var i = 1;

					foreach (var cashier in cashiers)
					{
						var item = new ComboBoxItem
						{
							Content = $"[{cashier.Id}] {cashier.GetFullName()}",
							DataContext = cashier
						};
						TimeSlotCashier.Items.Add(item);

						if (CashierExists() && cashier.Id == Cashier.Id)
							TimeSlotCashier.SelectedIndex = i;

						i++;
					}

				}

				Fill();
			}



			Loaded += (sender, args) => { Starting = false; };
		}

		private bool SubstituteExists() => Substitute != null;

		private bool CashierExists() => Cashier != null;

		private void TogglePause()
		{
			var pause = TimeSlot.Pause = !TimeSlot.Pause;

			TimeSlotCashier.IsEnabled = !pause;
			TimeSlotSubstitute.IsEnabled = !pause;


			//		TimeSlotSubstituteCashier.IsEnabled = SubstituteExists() ? (Substitute.Active ? 
	

		

		}

		private void ToggleSubstitute(bool toggle)
		{

			/*
			CheckSubstituteTimeSlot();
            if (toggle && !SubstituteTimeSlot.Substitute) return;
            TimeSlotSubstituteStart.IsEnabled = toggle;
            TimeSlotSubstituteEnd.IsEnabled = toggle;
            TimeSlotSubstituteCashier.IsEnabled = toggle; */
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

			//      if (CashierExists())
			//        TimeSlotCashierLastConnection.Text = Cashier.LastConnection.ToString("f");

			// set and find substitute
			if (!SubstituteExists()) return;

			//TimeSlotSubstituteCashier.Content = $"{SubstituteTimeSlot.}"
			// button set name of substitute if exists


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
									   //var substitute = SubstituteTimeSlot.Substitute = !SubstituteTimeSlot.Substitute;
									   //ToggleSubstitute(substitute);
		}

		private void CheckSubstituteTimeSlot()
		{
			/*if (SubstituteTimeSlot != null) return;

            SubstituteTimeSlot = new SaveableSubstituteTimeSlot
            {
                TimeSlot = this.TimeSlot
            };*/

		}
	}
}