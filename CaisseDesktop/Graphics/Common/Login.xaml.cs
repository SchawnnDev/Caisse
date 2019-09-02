using System;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Models.Checkouts.Common;
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

	        DataContext = new LoginModel();
	        ((LoginModel) DataContext).CloseAction = Close;

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

    }
}