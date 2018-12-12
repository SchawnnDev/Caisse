using System.ComponentModel;
using System.Windows;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Graphics.Admin.Events.Pages;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    /// Interaction logic for EvenementManager.xaml
    /// </summary>
    public partial class EvenementManager
    {
        public SaveableEvent Evenement { set; get; }
        private bool IsBack { get; set; } = false;
        public EvenementBrowser ParentWindow { get; }
        public CustomPage CurrentPage { get; set; }

        public EvenementManager(EvenementBrowser parentWindow, SaveableEvent evenement)
        {
            InitializeComponent();
            Evenement = evenement;
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
            if (!CurrentPage.CanClose() && !IsBack) return;
            e.Cancel = true;
        }

        private void CreateCheckout_OnClick(object sender, RoutedEventArgs e)
        {
            new CheckoutManager(this, null).ShowDialog();
        }

        private void DisplayCheckouts_OnClick(object sender, RoutedEventArgs e)
        {
            var check = (CustomPage) MasterFrame.Content;

            if (check.Equals("CheckoutMainPage")) return;

            if (check != null && !check.CanClose()) return;

            CustomPage page = new EventCheckoutPage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
        }

        private void EditInfos_OnClick(object sender, RoutedEventArgs e)
        {
            if (((CustomPage)MasterFrame.Content).Equals("CheckoutMainPage")) return;
            CustomPage page = new EventMainPage(this);
            MasterFrame.Content = page;
            CurrentPage = page;
        }
    }
}