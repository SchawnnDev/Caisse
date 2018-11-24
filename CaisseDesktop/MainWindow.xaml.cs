using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace CaisseDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (var db = new CaisseServerContext())
            {
                //TEST
                var o = db.Database.Connection.ConnectionString;
                db.Database.CreateIfNotExists();
                db.PaymentMethods.Add(new SaveablePaymentMethod()
                {
                    Name = "Especes",
                    AcceptedDetails = "50 cents",
                    MinFee = 0.0
                });
                db.SaveChanges();
            }
        }
    }
}