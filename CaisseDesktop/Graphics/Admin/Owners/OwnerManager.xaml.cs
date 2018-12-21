using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseDesktop.Models;
using CaisseLibrary.Concrete.Owners;
using CaisseLibrary.Data;
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
        private PermissionModel Model => DataContext as PermissionModel;
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
            DataContext = new PermissionModel();

            if (New)
            {
                SaveableOwner = new SaveableOwner
                {
                    Event = ParentWindow.Evenement,
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

            LoadPermissions();
        }

        private void LoadPermissions()
        {
            var permissions = Model.Permissions?.SelectMany(t => SaveableOwner.Permissions.Split(','))
                .Select(t => new Permission(t)).ToList();
            if (permissions == null || permissions.Count == 0)
            {
                Model.Permissions = new ObservableCollection<Permission>();
                return;
            }

            Model.Permissions = new ObservableCollection<Permission>(permissions);
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
            SaveableOwner.Permissions = string.Join(",", Model.Permissions.Select(t => t.Value).ToList());

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
                db.Events.Attach(SaveableOwner.Event);
                db.Entry(SaveableOwner).State = New ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "Le résponsable a bien été crée !" : "Le résponsable a bien été enregistré !");

                if (New)
                {
                    if (ParentWindow.CurrentPage.Equals("EventOwnerPage"))
                        ((EventOwnerPage) ParentWindow.CurrentPage).Add(SaveableOwner);
                }
                else
                {
                    if (ParentWindow.CurrentPage.Equals("EventOwnerPage"))
                        ((EventOwnerPage) ParentWindow.CurrentPage).Update();
                }

                ToggleBlocked(true);
                Saved = true;
            });
        }

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

        private void DeletePermission_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessionAdmin.HasNotPermission("owners.permissions.delete"))
                return;

            var btn = sender as Button;

            if (btn?.DataContext is Permission permission)
            {
                var found = Model.Permissions.FirstOrDefault(t => t.Value.Equals(permission.Value));
                if (found != null)
                    Model.Permissions.Remove(found);
            }
            else
            {
                MessageBox.Show($"{btn} : la permission n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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

        private void PermissionAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessionAdmin.HasNotPermission("owners.permissions.add") || Check(Permission))
                return;

            var value = Permission.Text;

            if (Model.Permissions.Any(t => t.Value == value))
            {
                MessageBox.Show("Cette permission a déjà été ajoutée.");
                return;
            }

            Model.Permissions.Add(new Permission(value));
            Permission.Text = "";
            Permission.ClearValue(BorderBrushProperty);
        }

        private bool Check(TextBox box)
        {
            var str = box.Text;
            if (!string.IsNullOrWhiteSpace(str) && !str.Contains(" ")) return false;
            box.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
        }
    }
}