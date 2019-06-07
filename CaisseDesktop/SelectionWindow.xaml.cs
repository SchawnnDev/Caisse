using System;
using System.Collections.Generic;
using System.Windows;
using CaisseDesktop.Graphics;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Graphics.Admin.Statistics;
using CaisseDesktop.Graphics.Common;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop
{
    /// <summary>
    /// Interaction logic for SelectionPage.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        public SelectionWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var browser = new EvenementBrowser();
            browser.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {// display statistics
	        new StatisticsMain().Show();
            Close();
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            Close();
        }
    }
}
