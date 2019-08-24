﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Utils;
using CaisseLibrary;
using CaisseLibrary.Exceptions;
using CaisseLibrary.IO;
using CaisseServer;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {
        private bool CanLogin { get; set; } = false;

        public Login(bool startup)
        {
            InitializeComponent();


            Loaded += (sender, args) =>
            {
                var config = ConfigFile.GetConfig();

                if (!config.ContainsKey("event"))
                {
                    new Parameters(this).ShowDialog();
                }
                else if (config.ContainsKey("checkout"))
                {
                    var eventId = int.Parse(config["event"]);
                    var checkoutId = int.Parse(config["checkout"]);

                    using (var db = new CaisseServerContext())
                    {
                        if (db.Events.Any(t => t.Id == eventId) && db.Checkouts.Any(t => t.Id == checkoutId))
                        {
                            Main.ActualEvent = db.Events.Single(t => t.Id == eventId);
                            Main.ActualCheckout = db.Checkouts.Where(t => t.Id == checkoutId)
                                .Include(t => t.CheckoutType).First();

                            Task.Run(() => ConfigureApp(startup == false));

                            UpdateLabels();
                        }
                        else
                        {
                            new Parameters(this).ShowDialog();
                        }
                    }
                }
                else
                {
                    new Parameters(this).ShowDialog();
                }

            };

        }

        public void ConfigureApp(bool reconfigure)
        {
            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                CanLogin = false;
                PrinterStatusLabel.Content = "Imprimante : Configuration de la connexion...";
            });

	        var changeStatus = true;

            try
            {
                if (reconfigure)
                {
                    Main.Reconfigure("TicketsPrinter"); //TODO
                }
                else
                {
                    Main.ConfigureCheckout("TicketsPrinter"); //TODO
                }
            }
            catch (TicketPrinterException e)
            {
                Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    if(!Validations.ShowErrorRequest($"{e.Message} Voulez vous continuer sans l'imprimante?")) { 
						Close();
						return;
                    }

	                PrinterStatusLabel.Content = "Imprimante : Pas de connexion";
	                changeStatus = false;

                });

            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                CanLogin = true;
				if(changeStatus)
					PrinterStatusLabel.Content = "Imprimante : OK";
            });
        }

        public void UpdateLabels()
        {
            CheckoutNameLabel.Content = $"Caisse: {Main.ActualCheckout.Name}";
        }

        private void OpenParameters_OnClick(object sender, RoutedEventArgs e)
        {

            if (!CanLogin)
            {
                SystemSounds.Beep.Play();
                Validations.ShowWarning("Veuillez attendre que l'application charge.");
                return;
            }

            new Parameters(this).ShowDialog();
        }

        private void Valider_OnClick(object sender, RoutedEventArgs e)
        {

            if (!CanLogin)
            {
                SystemSounds.Beep.Play();
                Validations.ShowWarning("Veuillez attendre que l'application charge.");
                return;
            }

            if (Password.Password.Length != 7)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("L'identifiant n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                /*
                new Checkout(Main.ActualCheckout).Show();
                Close();

                return; */


                var cashier = Main.Login(Password.Password);

                if (cashier == null)
                {
                    MessageBox.Show("L'identifiant n'est pas valide.", "Erreur", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                Main.ActualCashier = cashier;
                new Checkout(Main.ActualCheckout).Show();
                Close();


                //TODO: Si ce n'est pas encore l'heure du caissier, le prevenir et demander si il est sûr de vouloir continuer
                //MessageBox.Show("Bien connecté.", "Yeah", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}