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
using CaisseDesktop.Models.Admin;
using CaisseLibrary.Data;
using CaisseServer;
using CaisseServer.Events;
using EventManager = CaisseDesktop.Graphics.Admin.Events.EventManager;

namespace CaisseDesktop.Graphics.Admin.PaymentMethods
{
    /// <summary>
    /// Interaction logic for PaymentMethodManager.xaml
    /// </summary>
    public partial class PaymentMethodManager : Window
    {
        public EventManagerModel ParentWindow { get; set; }
        public SaveablePaymentMethod PaymentMethod { get; set; }
        private bool New { get; } = true;

        public PaymentMethodManager(EventManagerModel parentWindow, SaveablePaymentMethod paymentMethod)
        {
            InitializeComponent();
            ParentWindow = parentWindow;
            PaymentMethod = paymentMethod;
            New = paymentMethod == null;

            if (New)
            {
                PaymentMethod = new SaveablePaymentMethod
                {
                };
            }
            else
            {
                FillTextBoxes();
                New = false;
            }
        }

        private void FillTextBoxes()
        {
        }
    }
}