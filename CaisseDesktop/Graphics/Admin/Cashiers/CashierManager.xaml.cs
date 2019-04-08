using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CaisseDesktop.Admin;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Admin.TimeSlots;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseLibrary.Concrete.Owners;
using CaisseLibrary.Data;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Cashiers
{
    /// <summary>
    ///     Interaction logic for OwnerManager.xaml
    /// </summary>
    public partial class CashierManager
    {
        public CashierManager(TimeSlotManager parentWindow, SaveableTimeSlot TimeSlot)
        {
            InitializeComponent();
            ParentWindow = parentWindow;
            Cashier = TimeSlot.Cashier;
            New = Cashier == null;
            Closing += OnWindowClosing;

            if (New)
            {
                Cashier = new SaveableCashier
                {
                };
                Saved = false;
                Blocage.IsChecked = false;
            }
            else
            {
                FillTextBoxes();
                New = false;
                Saved = true;
                ToggleBlocked(true);
            }
        }

        private SaveableTimeSlot TimeSlot { get; set; }
        public TimeSlotManager ParentWindow { get; set; }
        public SaveableCashier Cashier { get; set; }
        private bool Saved { get; set; }
        private bool New { get; } = true;
        private bool Blocked { get; set; }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (Saved || !Saved && Validations.WillClose(true)) return;
            e.Cancel = true;
        }

        private void ToggleBlocked(bool blocked)
        {
            CashierFirstName.IsEnabled = !blocked;
            CashierName.IsEnabled = !blocked;
            GenLogin.IsEnabled = !blocked;
            CashierSave.IsEnabled = !blocked;
            Blocage.IsChecked = blocked;
            Blocked = blocked;
        }

        private void FillTextBoxes()
        {
            CashierFirstName.Text = Cashier.FirstName;
            CashierName.Text = Cashier.Name;
            CashierLastActivity.Text =
                $"{Cashier.LastActivity.ToLongDateString()} {Cashier.LastActivity.ToShortTimeString()}";
            FillLogin();
        }

        private void FillLogin()
        {
            CashierLogin.Text = SessionAdmin.HasPermission("cashiers.login.show")
                ? Cashier.Login
                : new string('•', Cashier.Login.Length);
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (CustomPage.Check(CashierFirstName) || CustomPage.Check(CashierName)) return;

            if (string.IsNullOrWhiteSpace(Cashier.Login))
            {
                GenLogin.BorderBrush = Brushes.Red;
                SystemSounds.Beep.Play();
                return;
            }
            Cashier.FirstName = CashierFirstName.Text;
            Cashier.Name = CashierName.Text;

            if (New) Cashier.LastActivity = DateTime.Now;

            TimeSlot.Cashier = Cashier;
            Close();
        }

        private void GenLogin_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessionAdmin.HasNotPermission("owners.login.gen"))
                return;

            // possible characters : 123456789ABCXYZ/*- and length=7

            using (var db = new CaisseServerContext())
            {
                var logins = db.Cashiers.Select(t => t.Login).ToList();

                Cashier.Login = new CashierPassword().GenerateNoDuplicate(7, logins);

                FillLogin();
            }
        }

        /*
        // Le save se fera surement sur le timeslot 
        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                db.Entry(Cashier).State = New ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "Le caissier a bien été crée !" : "Le caissier a bien été enregistré !");

                // set cashier on parent window (timeslotmanager)
                //SessionAdmin.UpdateIfEdited(SaveableOwner);

                ToggleBlocked(true);
                Saved = true;
            });
        } */

        private void Blocage_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Saved)
            {
                MessageBox.Show("Veuillez enregistrer avant.", "Veuillez enregistrer", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                Blocage.IsChecked = false;
                return;
            }

            ToggleBlocked(false);
            Saved = false;
        }

    }
}