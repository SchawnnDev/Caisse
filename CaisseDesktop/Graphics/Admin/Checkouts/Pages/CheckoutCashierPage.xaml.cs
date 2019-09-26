using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaisseDesktop.Models.Admin.Checkouts;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    /// Interaction logic for CheckoutCashierPage.xaml
    /// </summary>
    public partial class CheckoutCashierPage
    {
        public CheckoutCashierPage(CheckoutManagerModel parentModel)
        {
            InitializeComponent();
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }

        public override string CustomName => "CheckoutCashierPage";

        public override void Update()
        {
        }

        public override void Add<T>(T item)
        {
        }

        public override bool CanClose() => true;

        public override bool CanBack() => true;
    }
}
