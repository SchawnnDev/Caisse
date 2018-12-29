using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseDesktop.Graphics.Admin.Owners;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts
{
    /// <summary>
    /// Interaction logic for CheckoutManager.xaml
    /// </summary>
    public partial class CheckoutManager
    {
        public SaveableCheckout Checkout { set; get; }
        private bool IsBack { get; set; } = false;
        public EvenementManager ParentWindow { get; }
        public CustomPage CurrentPage { get; set; }

        public CheckoutManager(EvenementManager parentWindow, SaveableCheckout checkout)
        {
            InitializeComponent();
            Checkout = checkout;
            ParentWindow = parentWindow;
            Closing += OnWindowClosing;
            EditInfos_OnClick(null, null);
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CurrentPage.CanBack()) return;

            IsBack = true;
            Close();
            ParentWindow.Show();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (IsBack || CurrentPage.CanClose()) return;
            e.Cancel = true;
        }

        private void CreateCheckout_OnClick(object sender, RoutedEventArgs e)
        {
            if (Checkout != null)
            {
               // new CheckoutManager(this, null).ShowDialog();
                return;
            }

            if (!MasterFrame.ToCustomPage().Equals("EventMainPage")) return;

            SystemSounds.Beep.Play();
            MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EditInfos_OnClick(object sender, RoutedEventArgs e)
        {
            if (MasterFrame.Content != null && !MasterFrame.ToCustomPage().CanOpen("CheckoutMainPage")) return;
            CustomPage page = new CheckoutMainPage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(0);
        }

        /*
        private void DisplayOwners_OnClick(object sender, RoutedEventArgs e)
        {
            if (MasterFrame.Content != null && !MasterFrame.ToCustomPage().CanOpen("EventOwnerPage")) return;
            CustomPage page = new EventOwnerPage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(1);
        } */

        private void CreateCashier_OnClick(object sender, RoutedEventArgs e)
        {
            if (Checkout  != null)
            {
               // new OwnerManager(this, null).ShowDialog();
                return;
            }

            if (!MasterFrame.ToCustomPage().Equals("CheckoutMainPage")) return;

            SystemSounds.Beep.Play();
            MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private List<MenuItem> GetMenuItems() => new List<MenuItem>
        {
            EditInfos,
            DisplayCashiers,
            DisplayEdt
        };

        private void DisplayEdt_OnClick(object sender, RoutedEventArgs e)
        {
            var check = MasterFrame.ToCustomPage();

            if (!check.CanOpen("CheckoutTimeTablePage")) return;

            if (check != null && !check.CanClose()) return;

            CustomPage page = new CheckoutTimeTablePage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(2);
        }
    }
}
 