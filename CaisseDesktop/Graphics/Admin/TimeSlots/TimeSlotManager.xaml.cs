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
using CaisseDesktop.Models;
using CaisseDesktop.Models.Windows;
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
        private bool Starting { get; set; } = true;

	    private TimeSlotManagerModel Model => DataContext as TimeSlotManagerModel;

		public TimeSlotManager(CheckoutManager parentWindow,SaveableTimeSlot timeSlot)
        {
            InitializeComponent();
            ParentWindow = parentWindow;
            Owner = parentWindow;

	        DataContext = new TimeSlotManagerModel(timeSlot);

            Loaded += (sender, args) => { Starting = false; };
        }


        private string CorrectMissingZero(string time)
        {
            return time.Length == 3 ? $"0{time}" : time;
        }


        private void TimeSlotCashier_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Starting || e.AddedItems.Count != 1 || !(e.AddedItems[0] is ComboBoxItem item)) return;

            if (item.Content.Equals("Aucun"))
            {
	            Model.TimeSlot.Cashier = null;
            }
            else if (item.DataContext != null && (item.DataContext is SaveableCashier cashier))
            {
	            Model.TimeSlot.Cashier = cashier;
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
     //       if (CustomPage.Check(TimeSlotStart, Start, End) || CustomPage.Check(TimeSlotEnd, Start, End))
       //         return;

            // Pause is first priority

            Debug.Assert(TimeSlotPause.IsChecked != null, "TimeSlotPause.IsChecked != null");

         //   TimeSlot.Pause = TimeSlotPause.IsChecked.Value;
           // TimeSlot.Start = TimeSlotStart.SelectedTime ?? Start;
           // TimeSlot.End = TimeSlotEnd.SelectedTime ?? End;
            Task.Run(() => Save());
        }


        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
				/*     if (CashierExists())
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

					 if (New)
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

					 db.Entry(TimeSlot).State = New ? EntityState.Added : EntityState.Modified;
					 */
				db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
             //   MessageBox.Show(New ? "Le créneau horaire a bien été crée !" : "Le créneau horaire a bien été enregistré !");
                ParentWindow.MasterFrame.ToCustomPage().Update();
                Close();
            });
        }

        private void TimeSlotCashier_OnClick(object sender, RoutedEventArgs e)
        {
            new CashierManager(this, Model.TimeSlot, false).ShowDialog();
        }

        private void TimeSlotSubstituteCashier_OnClick(object sender, RoutedEventArgs e)
        {
            new CashierManager(this, Model.TimeSlot, true).ShowDialog();
        }

        public void SetCashier(SaveableCashier cashier)
        {
            if (cashier.Substitute)
            {
                Model.TimeSlot.Substitute = cashier;
                TimeSlotSubstituteCashier.Content = cashier.GetFullName();
                return;
            }

	        Model.TimeSlot.Cashier = cashier;
            TimeSlotCashier.Content = cashier.GetFullName();
        }

        public void RemoveCashier(SaveableCashier cashier)
        {
            if (cashier == null) return;

            if (cashier.Substitute) Model.TimeSlot.Substitute = null;
            else Model.TimeSlot.Cashier = null;

            if (cashier.Substitute)
            {
                TimeSlotSubstitute.Content = "Créer";
            }
            else
            {
                TimeSlotCashier.Content = "Créer";
            }

          //  if (New) return;

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