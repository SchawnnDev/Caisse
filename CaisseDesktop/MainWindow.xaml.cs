using System;
using System.Collections.Generic;
using System.Windows;
using CaisseDesktop.Graphics;
using CaisseDesktop.Graphics.Admin;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Common;
using CaisseDesktop.Graphics.Print;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Concrete.Session;
using CaisseServer;
using CaisseServer.Items;

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

            Main.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var browser = new EvenementBrowser();
            browser.Show();
            Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var ticket = new SalesReceipt(new Invoice(CashierSession.ActualCashier)
            {

                SaveableInvoice = new SaveableInvoice
                {
                    Cashier = new SaveableCashier
                    {
                        Id = 12
                    },

                    Id = 14808
                    ,
                    PaymentMethod = new SaveablePaymentMethod
                    {
                        Name = "ESPECE"
                    },

                    Date = DateTime.Now

                },

                GivenMoney = 50m,

                Operations = new List<SaveableOperation>
                {
                    new SaveableOperation
                    {
                        Amount = 5,
                        Item = new SaveableArticle
                        {
                            Name = "CREPE",
                            Price = 2m
                        }
                    }, new SaveableOperation
                    {
                        Amount = 2,
                        Item = new SaveableArticle
                        {
                            Name = "PIZZA",
                            Price = 6.5m
                        }
                    }, new SaveableOperation
                    {
                        Amount = 4,
                        Item = new SaveableArticle
                        {
                            Name = "BOISSON",
                            Price = 2.5m
                        }
                    }, new SaveableOperation
                    {
                        Amount = 1,
                        Item = new SaveableArticle
                        {
                            Name = "BIERE",
                            Price = 3.5m
                        }
                    },
                }

            });
            ticket.Generate();
            ticket.Print();
        }

        private void ButtonConnection_OnClick(object sender, RoutedEventArgs e)
        {
            new Connection().Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Checkout().Show();
            Close();
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            Close();
        }
    }
}