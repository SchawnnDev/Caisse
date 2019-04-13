using System;
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
        private Timer UpdateTimer { get; set; }
        private bool CanLogin { get; set; } = false;

        public Login()
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

                            Task.Run(() => ConfigureApp(false));

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


                UpdateTimer = new Timer(1000) {AutoReset = true, Enabled = true}; // 1000 ms => 1 sec
                UpdateTimer.Elapsed += (o, eventArgs) =>
                {
                    DateLabel.Dispatcher.Invoke(() =>
                    {
                        DateLabel.Content = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
                    });
                };
            };

            Closed += OnClosed;
        }

        public void ConfigureApp(bool reconfigure)
        {
            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                CanLogin = false;
                PrinterStatusLabel.Content = "Imprimante : Configuration de la connexion...";
            });

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
                    Validations.ShowError(e.Message);
                    Close();
                });
                return;
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                CanLogin = true;
                PrinterStatusLabel.Content = "Imprimante : OK";
            });
        }

        public void UpdateLabels()
        {
            CheckoutNameLabel.Content = $"Caisse: {Main.ActualCheckout.Name}";
        }

        private void OnClosed(object sender, EventArgs e)
        {
            if (UpdateTimer == null) return;
            UpdateTimer.Stop();
            UpdateTimer.Dispose();
        }

        private void OpenParameters_OnClick(object sender, RoutedEventArgs e)
        {
            new Parameters(this).ShowDialog();
        }

        private void PinPadButton_Click(object sender, RoutedEventArgs e)
        {
            var buttonContent = (sender as Button)?.Content;

            if (buttonContent == null) return;

            var str = buttonContent.ToString();

            if (int.TryParse(str, out var number))
            {
                if (Password.Password.Length < 7)
                    Password.Password = Password.Password += number;
                else
                    SystemSounds.Beep.Play();
            }
            else if (str.EndsWith("Suppr."))
            {
                var len = Password.Password.Length;

                if (len != 0)
                    Password.Password = Password.Password.Remove(len - 1);
                else
                    SystemSounds.Beep.Play();
            }
            else if (str.EndsWith("Annuler"))
            {
                Password.Password = "";
            }
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