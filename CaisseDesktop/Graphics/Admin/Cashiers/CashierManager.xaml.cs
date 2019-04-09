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
        public CashierManager(TimeSlotManager parentWindow, SaveableTimeSlot timeSlot, bool substitute)
        {
            InitializeComponent();
            Owner = parentWindow;
            ParentWindow = parentWindow;
            Cashier = substitute ? timeSlot.Substitute : timeSlot.Cashier;
            New = Cashier == null;
            Closing += OnWindowClosing;

            if (New)
            {
                Cashier = new SaveableCashier
                {
                    Substitute = substitute,
                    Checkout = timeSlot.Checkout // Maybe remove this (???)
                };
                Saved = false;
            }
            else
            {
                FillTextBoxes();
                New = false;
                Saved = true;
                CashierDelete.IsEnabled = true;
            }
        }

        public TimeSlotManager ParentWindow { get; set; }
        public SaveableCashier Cashier { get; set; }
        private bool Saved { get; set; }
        private bool New { get; } = true;

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (Saved || !Saved && Validations.WillClose(true)) return;
            e.Cancel = true;
        }

        private void FillTextBoxes()
        {
            CashierFirstName.Text = Cashier.FirstName;
            CashierName.Text = Cashier.Name;
            CashierLastActivity.Text =
                $"{Cashier.LastActivity.ToLongDateString()} {Cashier.LastActivity.ToShortTimeString()}";
            CashierWasHere.IsChecked = Cashier.WasHere;
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

            Saved = true;

            // set cashier or substitute
            ParentWindow.SetCashier(Cashier);

            // direct close of dialog
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

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Es tu sûr de vouloir supprimer ce caissier ?", "Supprimer un caissier",
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result != MessageBoxResult.Yes) return;

            // remove cashier

            ParentWindow.RemoveCashier(Cashier);

            Saved = true;

            // direct close of dialog
            Close();
        }

        private void CashierWasHere_OnClick(object sender, RoutedEventArgs e)
        {
            Cashier.WasHere = !Cashier.WasHere;
        }
    }
}