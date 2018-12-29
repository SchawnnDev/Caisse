using System.Windows;
using CaisseDesktop.Graphics;
using CaisseDesktop.Graphics.Admin;
using CaisseDesktop.Graphics.Common;
using CaisseDesktop.Graphics.Print;
using CaisseServer;

namespace CaisseDesktop
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (var db = new CaisseServerContext())
            {
                //TEST

                db.Database.CreateIfNotExists();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new AdminMain();
            window.Show();
            Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var ticket = new SalesReceipt();
            ticket.Generate();
            ticket.Print();
        }

        private void ButtonConnection_OnClick(object sender, RoutedEventArgs e)
        {
            new Connection().ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Checkout().Show();
            Close();
        }
    }
}