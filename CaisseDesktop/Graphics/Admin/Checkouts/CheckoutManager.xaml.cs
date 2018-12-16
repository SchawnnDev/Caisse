using System.Windows;
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Checkouts
{
    /// <summary>
    /// Interaction logic for CheckoutManager.xaml
    /// </summary>
    public partial class CheckoutManager
    {
        public EvenementManager EventManager { get; set; }
        public SaveableCheckout Checkout { get; set; }
        private bool Saved { get; set; } = false;
        private bool New { get; set; } = true;
        
        public CheckoutManager(EvenementManager eventManager, SaveableCheckout checkout)
        {
            InitializeComponent();
            EventManager = eventManager;
            Checkout = checkout;
            MasterFrame.Content = new CheckoutMainPage(this);
            New = checkout == null;
            Saved = !New;
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DisplayEdt_OnClick(object sender, RoutedEventArgs e)
        {
            MasterFrame.Content = new DaysDisplayPage();
        }
    }
}
 