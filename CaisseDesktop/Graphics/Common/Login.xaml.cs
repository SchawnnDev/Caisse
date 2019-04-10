using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseDesktop.Client;
using CaisseLibrary;
using CaisseLibrary.Concrete.Session;
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
                            CheckoutSession.ActualCheckout = db.Checkouts.Where(t => t.Id == checkoutId)
                                .Include(t => t.CheckoutType).First();
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
                        DateLabel.Content = DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy");
                    });
                };
            };

            Closed += OnClosed;
        }

        public void UpdateLabels()
        {
            CheckoutNameLabel.Content = $"Caisse: {CheckoutSession.ActualCheckout.Name}";
        }

        private void OnClosed(object sender, EventArgs e)
        {
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
            if (Password.Password.Length != 7)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("L'identifiant n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                new Checkout(CheckoutSession.ActualCheckout).Show();
                Close();

                return;


                var cashier = ClientMain.Login(Password.Password);

                if (cashier == null)
                {
                    MessageBox.Show("L'identifiant n'est pas valide.", "Erreur", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }


                //TODO: Si ce n'est pas encore l'heure du caissier, le prevenir et demander si il est sûr de vouloir continuer
                //MessageBox.Show("Bien connecté.", "Yeah", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
    }
}