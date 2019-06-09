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
using CaisseDesktop.Models.Windows;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Statistics
{
    /// <summary>
    /// Interaction logic for DisplayInvoice.xaml
    /// </summary>
    public partial class DisplayInvoice : Window
    {

        private DisplayInvoiceModel Model => DataContext as DisplayInvoiceModel;

        public DisplayInvoice(SaveableInvoice invoice)
        {
            InitializeComponent();

            DataContext = new DisplayInvoiceModel(invoice);
        }
    }
}
