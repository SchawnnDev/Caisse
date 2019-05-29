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
using System.Windows.Shapes;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Windows;
using CaisseLibrary.Data;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.PaymentMethods
{
    /// <summary>
    /// Interaction logic for PaymentMethodManager.xaml
    /// </summary>
    public partial class PaymentMethodManager : Window
    {
        public SaveablePaymentMethod PaymentMethod { get; set; }

	    private EventMainPageModel Model => DataContext as EventMainPageModel;

		public PaymentMethodManager(EvenementManager parentWindow, SaveablePaymentMethod paymentMethod)
        {
            InitializeComponent();
	        Owner = parentWindow;
            PaymentMethod = paymentMethod;
	        DataContext = new PaymentMethodManagerModel(paymentMethod);
		}

    }
}