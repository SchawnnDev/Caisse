using System;
using System.Collections.Generic;
using System.Linq;
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
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Checkouts
{
    /// <summary>
    /// Interaction logic for CheckoutManager.xaml
    /// </summary>
    public partial class CheckoutManager : Window
    {
        private EvenementManager EventManager { get; set; }
        private SaveableCheckout Checkout { get; set; }
        
        public CheckoutManager(EvenementManager eventManager, SaveableCheckout checkout)
        {
            InitializeComponent();
            EventManager = eventManager;
            Checkout = checkout;
            ContentControl.Content = new DaysDisplayPage();
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
