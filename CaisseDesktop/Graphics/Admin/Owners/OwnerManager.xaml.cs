using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
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
using CaisseDesktop.Admin;
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseLibrary.Concrete.Owners;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Owners
{
    /// <summary>
    /// Interaction logic for OwnerManager.xaml
    /// </summary>
    public partial class OwnerManager
    {
        public EvenementManager ParentWindow { get; set; }
        public SaveableOwner SaveableOwner { get; set; }
        private bool Saved { get; set; }
        private bool New { get; set; } = true;
        private bool Blocked { get; set; }

        public OwnerManager(EvenementManager parentWindow, SaveableOwner owner)
        {
            InitializeComponent();
            ParentWindow = parentWindow;
            SaveableOwner = owner;
            New = owner == null;

            if (New)
            {
                SaveableOwner = new SaveableOwner
                {
                    SuperAdmin = false,
                    Permissions = ""
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

        private void ToggleBlocked(bool blocked)
        {
            OwnerName.IsEnabled = !blocked;
            OwnersGrid.IsEnabled = !blocked;

            if (SessionAdmin.HasPermission("owners.login.gen"))
                GenLogin.IsEnabled = !blocked;

            Blocage.IsChecked = blocked;
            Blocked = blocked;
        }

        private void FillTextBoxes()
        {
            OwnerName.Text = SaveableOwner.Name;
            OwnerConnect.Text =
                $"{SaveableOwner.LastLogin.ToLongDateString()} {SaveableOwner.LastLogin.ToShortTimeString()}";
            FillLogin();
        }

        private void FillLogin()
        {
            if (SessionAdmin.HasPermission("owners.login.show"))
            {
                OwnerLogin.Text = SaveableOwner.Login;
                return;
            }

            OwnerLogin.Text = new string('•', SaveableOwner.Login.Length);
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (CustomPage.Check(OwnerName)) return;

            if (string.IsNullOrWhiteSpace(SaveableOwner.Login))
            {
                GenLogin.BorderBrush = Brushes.Red;
                SystemSounds.Beep.Play();
                return;
            }

            SaveableOwner.Name = OwnerName.Text;

            if (New)
            {
                var now = DateTime.Now;
                SaveableOwner.LastLogin = now;
                SaveableOwner.LastLogout = now;
            }


            Task.Run(() => Save());
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                db.Owners.AddOrUpdate(SaveableOwner);
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "Le résponsable a bien été crée !" : "Le résponsable a bien été enregistré !");
                if (New) ParentWindow.ParentWindow.Add(ParentWindow.Evenement);
                else ParentWindow.ParentWindow.Update();
                ToggleBlocked(true);
                Saved = true;
            });
        }

        private void Blocage_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Saved)
            {
                MessageBox.Show("Veuillez enregistrer avant.");
                Blocage.IsChecked = false;
                return;
            }

            ToggleBlocked(false);
            Saved = false;
        }

        private void DeletePermission_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessionAdmin.HasNotPermission("owners.permissions.delete"))
                return;
        }

        private void ToggleSuperAdmin_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessionAdmin.HasNotPermission("owners.superadmin"))
                return;

            var admin = !OwnerSuperAdmin.IsChecked ?? false;

            SaveableOwner.SuperAdmin = admin;
            OwnerSuperAdmin.IsChecked = admin;
        }

        private void GenLogin_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessionAdmin.HasNotPermission("owners.login.gen"))
                return;

            // possible characters : 123456789ABCXYZ/*- and length=7

            using (var db = new CaisseServerContext())
            {
                var logins = db.Owners.Select(t => t.Login).ToList();

                SaveableOwner.Login = new OwnerPassword().GenerateNoDuplicate(7, logins);

                FillLogin();
            }
        }
    }
}