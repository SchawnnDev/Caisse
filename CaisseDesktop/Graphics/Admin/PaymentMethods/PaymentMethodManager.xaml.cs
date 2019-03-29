﻿using System;
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
        public EvenementManager ParentWindow { get; set; }
        public SaveablePaymentMethod PaymentMethod { get; set; }
        private bool New { get; } = true;

        public PaymentMethodManager(EvenementManager parentWindow, SaveablePaymentMethod paymentMethod)
        {
            InitializeComponent();
            ParentWindow = parentWindow;
            PaymentMethod = paymentMethod;
            New = paymentMethod == null;

            if (New)
            {
                PaymentMethod = new SaveablePaymentMethod
                {
                    Event = parentWindow.Evenement
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