using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseDesktop.Graphics.Admin.Owners;
using CaisseDesktop.Utils;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    ///     Interaction logic for EvenementManager.xaml
    /// </summary>
    public partial class EvenementManager
    {
        public EvenementManager(EvenementBrowser parentWindow, SaveableEvent evenement)
        {
            InitializeComponent();
            Evenement = evenement;
            ParentWindow = parentWindow;
            Closing += OnWindowClosing;
            EditInfos_OnClick(null, null);
        }

        public SaveableEvent Evenement { set; get; }
        private bool IsBack { get; set; }
        public EvenementBrowser ParentWindow { get; }
        public CustomPage CurrentPage { get; set; }

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
            if (Evenement != null)
            {
                new CheckoutManager(this, null).ShowDialog();
                return;
            }

            if (!MasterFrame.ToCustomPage().Equals("EventMainPage")) return;

            SystemSounds.Beep.Play();
            MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DisplayCheckouts_OnClick(object sender, RoutedEventArgs e)
        {
            var check = MasterFrame.ToCustomPage();

            if (!check.CanOpen("EventCheckoutPage")) return;

            if (check != null && !check.CanClose()) return;

            CustomPage page = new EventCheckoutPage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(0);
        }

        private void EditInfos_OnClick(object sender, RoutedEventArgs e)
        {
            if (MasterFrame.Content != null && !MasterFrame.ToCustomPage().CanOpen("EventMainPage")) return;
            CustomPage page = new EventMainPage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(2);
        }

        private void DisplayOwners_OnClick(object sender, RoutedEventArgs e)
        {
            if (MasterFrame.Content != null && !MasterFrame.ToCustomPage().CanOpen("EventOwnerPage")) return;
            CustomPage page = new EventOwnerPage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(1);
        }

        private void CreateOwner_OnClick(object sender, RoutedEventArgs e)
        {
            if (Evenement != null)
            {
                new OwnerManager(this, null).ShowDialog();
                return;
            }

            if (!MasterFrame.ToCustomPage().Equals("EventMainPage")) return;

            SystemSounds.Beep.Play();
            MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                DisplayCheckouts,
                DisplayOwners,
                EditInfos,
                DisplayCheckoutTypes
            };
        }

        private void DisplayCheckoutTypes_OnClick(object sender, RoutedEventArgs e)
        {
            if (MasterFrame.Content != null && !MasterFrame.ToCustomPage().CanOpen("EventOwnerPage")) return;
            CustomPage page = new EventCheckoutTypePage();
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(3);
        }

        private void CreateCheckoutType_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}