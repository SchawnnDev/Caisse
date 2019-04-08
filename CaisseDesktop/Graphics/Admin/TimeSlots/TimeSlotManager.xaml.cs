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
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.TimeSlots
{
    /// <summary>
    /// Interaction logic for TimeSlotManager.xaml
    /// </summary>
    public partial class TimeSlotManager
    {

        private SaveableTimeSlot TimeSlot { get; set; }
        private readonly DateTime Start;
        private readonly DateTime End;
        private bool New { get; }
        private bool Starting { get; set; } = true;
       

        public TimeSlotManager(SaveableTimeSlot timeSlot, SaveableDay day, SaveableCheckout checkout, DateTime start,
            DateTime end)
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

                    var cashiers = db.Cashiers.Where(t => t.Checkout.Id == timeSlot.Checkout.Id).ToList();

                    var i = 1;

                    foreach (var cashier in cashiers)
                    {
                        var item = new ComboBoxItem
                        {
                            Content = $"[{cashier.Id}] {cashier.GetFullName()}",
                            DataContext = cashier
                        };
                        TimeSlotCashier.Items.Add(item);

                        if (CashierExists() && cashier.Id == TimeSlot.Cashier.Id)
                            TimeSlotCashier.SelectedIndex = i;

                        i++;
                    }
                }
            }

            Fill(); // Fill

            Loaded += (sender, args) => { Starting = false; };
        }

        private bool SubstituteExists() => TimeSlot.Substitute != null;

        private bool CashierExists() => TimeSlot.Cashier != null;

        private void TogglePause()
        {
            var pause = TimeSlot.Pause = !TimeSlot.Pause;

            TimeSlotCashier.IsEnabled = !pause;
            TimeSlotSubstitute.IsEnabled = !pause;

            TimeSlotSubstituteCashier.IsEnabled = !pause && SubstituteExists() && TimeSlot.Substitute.Active;

            TimeSlotSubstitute.IsEnabled = !pause;
        }

        private void ToggleSubstitute(bool toggle)
        {
            if (SubstituteExists())
                TimeSlot.Substitute.Active = toggle;

            TimeSlotCashier.IsEnabled = !toggle;
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

            //      if (CashierExists())
            //        TimeSlotCashierLastConnection.Text = Cashier.LastConnection.ToString("f");

            // set and find substitute
            if (!SubstituteExists()) return;

            TimeSlotSubstituteCashier.Content = $"[{TimeSlot.Substitute.Id}] {TimeSlot.Substitute.GetFullName()}";
            // button set name of substitute if exists
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
            //CheckSubstituteTimeSlot(); // important
            //var substitute = SubstituteTimeSlot.Substitute = !SubstituteTimeSlot.Substitute;
            //ToggleSubstitute(substitute);
            ToggleSubstitute(TimeSlotSubstitute.IsChecked ?? false);
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (CustomPage.Check(TimeSlotStart, Start, End) || CustomPage.Check(TimeSlotEnd, Start, End))
                return;

            // Pause is first priority

            Debug.Assert(TimeSlotPause.IsChecked != null, "TimeSlotPause.IsChecked != null");

            var pause = TimeSlotPause.IsChecked.Value;

            if (!pause && CashierExists())
            {

            }



            TimeSlot.Pause = pause;
            Task.Run(() => Save());

        }


        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {

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

                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "L'article a bien été crée !" : "L'article a bien été enregistré !");
            });
        }
    }
}