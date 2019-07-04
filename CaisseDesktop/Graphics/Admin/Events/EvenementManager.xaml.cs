using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Graphics.Admin.CheckoutTypes;
using CaisseDesktop.Graphics.Admin.Days;
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseDesktop.Graphics.Admin.Owners;
using CaisseDesktop.Graphics.Admin.PaymentMethods;
using CaisseDesktop.Graphics.Print;
using CaisseDesktop.Utils;
using CaisseLibrary.Data;
using CaisseServer;
using CaisseServer.Events;
using Microsoft.Win32;
using ProtoBuf;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    ///     Interaction logic for EvenementManager.xaml
    /// </summary>
    public partial class EvenementManager
    {
        public EvenementManager(SaveableEvent evenement)
        {
            InitializeComponent();
            Evenement = evenement;
            Closing += OnWindowClosing;
            EditInfos_OnClick(null, null);
        }

        public SaveableEvent Evenement { set; get; }
        private bool IsBack { get; set; }
        public CustomPage CurrentPage { get; set; }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CurrentPage.CanBack()) return;

            IsBack = true;
            Close();
            new EvenementBrowser().Show();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (IsBack || CurrentPage.CanClose()) return;
            e.Cancel = true;
        }

        private List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                DisplayCheckouts,
                DisplayOwners,
                EditInfos,
                DisplayCheckoutTypes,
                DisplayDays,
                DisplayPaymentMethods
            };
        }

        private void CreateCheckout_OnClick(object sender, RoutedEventArgs e)
        {
            if (Evenement != null)
            {
                new CheckoutManager(this, null).ShowDialog();
                return;
            }

            CheckInfos();
        }

        private void CreateOwner_OnClick(object sender, RoutedEventArgs e)
        {
            if (Evenement != null)
            {
                new OwnerManager(this, null).ShowDialog();
                return;
            }

            CheckInfos();
        }

        private void CreateCheckoutType_OnClick(object sender, RoutedEventArgs e)
        {
            if (Evenement != null)
            {
                new CheckoutTypeManager(this, null).ShowDialog();
                return;
            }

            CheckInfos();
        }

        private void CreateDay_OnClick(object sender, RoutedEventArgs e)
        {
            if (Evenement != null)
            {
                new DayManager(this, null).ShowDialog();
                return;
            }

            CheckInfos();
        }

        private void CreatePaymentMethod_OnClick(object sender, RoutedEventArgs e)
        {
            if (Evenement != null)
            {
                new PaymentMethodManager(this, null).ShowDialog();
                return;
            }

            CheckInfos();
        }

        private void DisplayCheckouts_OnClick(object sender, RoutedEventArgs e)
        {
            Display(new EventCheckoutPage(this), 0);
        }

        private void DisplayOwners_OnClick(object sender, RoutedEventArgs e)
        {
            Display(new EventOwnerPage(this), 1);
        }

        private void EditInfos_OnClick(object sender, RoutedEventArgs e)
        {
            Display(new EventMainPage(this), 2);
        }

        private void DisplayCheckoutTypes_OnClick(object sender, RoutedEventArgs e)
        {
            Display(new EventCheckoutTypePage(this), 3);
        }

        private void DisplayDays_OnClick(object sender, RoutedEventArgs e)
        {
            Display(new EventDayPage(this), 4);
        }

        private void DisplayPaymentMethods_OnClick(object sender, RoutedEventArgs e)
        {
            Display(new EventPaymentMethodPage(this), 5);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            if (Evenement == null) return;

	        var saveFileDialog = new SaveFileDialog
	        {
		        Title = "Select a folder",
		        CheckPathExists = true,
		        Filter = "Caisse Files|*.caisse"
	        };

	        if (saveFileDialog.ShowDialog() != true) return;

	        var path = saveFileDialog.FileName;

	        if (!path.EndsWith(".caisse"))
		        path += ".caisse";

	        var saveableEvent = new Event();

			using (var db = new CaisseServerContext())
				saveableEvent.From(Evenement, db);

	        using (var file = File.Create(path))
				Serializer.Serialize(file, saveableEvent);

	        MessageBox.Show("Le fichier a bien été enregistré !");
        }

        private void Display(CustomPage page, int navigation)
        {
            if (MasterFrame.Content != null && !MasterFrame.ToCustomPage().CanOpen(page.CustomName)) return;
            MasterFrame.Content = page;
            CurrentPage = page;
            GetMenuItems().DoPageNavigation(navigation);
        }

        private void CheckInfos()
        {
            if (!MasterFrame.ToCustomPage().Equals("EventMainPage")) return;

            SystemSounds.Beep.Play();
            MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}