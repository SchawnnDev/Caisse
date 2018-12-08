using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    /// Interaction logic for CheckoutMainPage.xaml
    /// </summary>
    public partial class CheckoutMainPage
    {
        private ObservableCollection<SaveableCheckoutType> types;
        private ObservableCollection<SaveableOwner> owners;

        public CheckoutMainPage()
        {
            InitializeComponent();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Blocage_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void CheckoutType_OnLostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void Owner_OnLostFocus(object sender, RoutedEventArgs e)
        {
        }

    }
}
