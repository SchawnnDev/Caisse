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
using CaisseDesktop.Graphics.Admin.Events;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Checkouts
{
    /// <summary>
    /// Interaction logic for CheckoutManager.xaml
    /// </summary>
    public partial class CheckoutManager : Window
    {
        private EvenementManager Parent { get; set; }
        private SaveableCheckout Checkout { get; set; }
        
        public CheckoutManager(EvenementManager parent, SaveableCheckout checkout)
        {
            InitializeComponent();
            Parent = parent;
            Checkout = checkout;
        }
    }
}
