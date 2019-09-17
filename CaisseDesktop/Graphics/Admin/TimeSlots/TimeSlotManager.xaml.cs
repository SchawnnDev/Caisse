using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Media;
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
using CaisseDesktop.Graphics.Admin.Cashiers;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.TimeSlots
{
	/// <summary>
	/// Interaction logic for TimeSlotManager.xaml
	/// </summary>
	public partial class TimeSlotManager
	{

		private CheckoutManager ParentWindow { get; set; }
		private SaveableTimeSlot TimeSlot { get; set; }
		private readonly DateTime Start;
		private readonly DateTime End;
		private bool Starting { get; set; } = true;


		public TimeSlotManager(CheckoutManager parentWindow, SaveableTimeSlot timeSlot, SaveableDay day)
		{
			InitializeComponent();
			ParentWindow = parentWindow;
			Owner = parentWindow;
			TimeSlot = timeSlot;
			Start = day.Start;
			End = day.End;

			Loaded += (sender, args) => { Starting = false; };
		}


		private void ToggleSubstitute(bool toggle)
		{
			if (SubstituteExists())
				TimeSlot.SubstituteActive = toggle;

		//	TimeSlotCashier.IsEnabled = !toggle;
		//	TimeSlotSubstituteCashier.IsEnabled = toggle;
		}


		private void TimeSlotCashier_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Starting || e.AddedItems.Count != 1 || !(e.AddedItems[0] is ComboBoxItem item)) return;

			if (item.Content.Equals("Aucun"))
			{
				TimeSlot.Cashier = null;
			}
			else if (item.DataContext != null && (item.DataContext is SaveableCashier cashier))
			{
				TimeSlot.Cashier = cashier;
			}
		}

		private void Save_OnClick(object sender, RoutedEventArgs e)
		{
			if (TimeSlot.Start.CompareTo(Start) < 0 || TimeSlot.Start.CompareTo(End) > 0)
			{
				//TimeSlotStart.BorderBrush = Brushes.Red;
				SystemSounds.Beep.Play();
			}

			if (TimeSlot.End.CompareTo(Start) < 0 || TimeSlot.End.CompareTo(End) > 0)
			{
				//TimeSlotEnd.BorderBrush = Brushes.Red;
				SystemSounds.Beep.Play();
			}

			// Pause is first priority

			//Debug.Assert(TimeSlotPause.IsChecked != null, "TimeSlotPause.IsChecked != null");

			/*TimeSlot.Pause = TimeSlotPause.IsChecked.Value;
			TimeSlot.Start = TimeSlotStart.SelectedTime ?? Start;
			TimeSlot.End = TimeSlotEnd.SelectedTime ?? End;
			TimeSlot.Start = ModifyTime(Start, TimeSlot.Start);
			TimeSlot.End = ModifyTime(End, TimeSlot.End); */
			Task.Run(() => Save());
		}


		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				if (CashierExists())
				{
					if (db.Cashiers.Any(t => t.Id == TimeSlot.Cashier.Id))
					{
						db.Cashiers.Attach(TimeSlot.Cashier);
						db.Entry(TimeSlot.Cashier).State = EntityState.Modified;
					}
					else
					{
						db.Checkouts.Attach(TimeSlot.Cashier.Checkout); // Attach checkouts (maybe remove???)
						db.Entry(TimeSlot.Cashier).State = EntityState.Added;
					}
				}

				if (SubstituteExists())
				{
					if (db.Cashiers.Any(t => t.Id == TimeSlot.Substitute.Id))
					{
						db.Cashiers.Attach(TimeSlot.Substitute);
						db.Entry(TimeSlot.Substitute).State = EntityState.Modified;
					}
					else
					{
						db.Checkouts.Attach(TimeSlot.Substitute.Checkout); // Attach checkouts (maybe remove???)
						db.Entry(TimeSlot.Substitute).State = EntityState.Added;
					}
				}

				if (TimeSlot.Blank)
				{
					db.Checkouts.Attach(TimeSlot.Checkout);
					db.Days.Attach(TimeSlot.Day);
					// Attach etc.

					db.TimeSlots.Add(TimeSlot);
				}
				else
				{
					db.TimeSlots.Attach(TimeSlot);
				}

				db.Entry(TimeSlot).State = TimeSlot.Blank ? EntityState.Added : EntityState.Modified;

				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = null;
				MessageBox.Show(TimeSlot.Blank ? "Le créneau horaire a bien été crée !" : "Le créneau horaire a bien été enregistré !");
				ParentWindow.MasterFrame.ToCustomPage().Update();
				Close();
			});
		}

        private bool CashierExists() => false;
        private bool SubstituteExists() => false;

		private void TimeSlotSubstituteCashier_OnClick(object sender, RoutedEventArgs e)
		{
			if (TimeSlot.Substitute == null)
			{
				TimeSlot.Substitute = new SaveableCashier
				{
					Substitute = true,
					Checkout = TimeSlot.Checkout, // Maybe remove this (???)
					LastActivity = DateTime.Now,
					WasHere = false
				};
			}

			new CashierManager(this, TimeSlot.Cashier).ShowDialog();
		}

		public void SetCashier(SaveableCashier cashier)
		{
			if (cashier.Substitute)
			{
				TimeSlot.Substitute = cashier;
			//	TimeSlotSubstituteCashier.Content = cashier.GetFullName();
				return;
			}

			TimeSlot.Cashier = cashier;
		//	TimeSlotCashier.Content = cashier.GetFullName();
		}

		private DateTime ModifyTime(DateTime date, DateTime value) => new DateTime(date.Year, date.Month, date.Day, value.Hour, value.Minute, value.Second);

		public void RemoveCashier(SaveableCashier cashier)
		{
			if (cashier == null) return;

			if (cashier.Substitute) TimeSlot.Substitute = null;
			else TimeSlot.Cashier = null;

			if (cashier.Substitute)
			{
			//	TimeSlotSubstitute.Content = "Créer";
			}
			else
			{
			//	TimeSlotCashier.Content = "Créer";
			}

			if (TimeSlot.Blank) return;

			Task.Run(() =>
			{
				using (var db = new CaisseServerContext())
				{
					if (db.Cashiers.Any(t => t.Id == cashier.Id))
						db.Cashiers.Remove(cashier);
					db.SaveChangesAsync();
				}
			});
		}
	}
}