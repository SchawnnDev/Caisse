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
using CaisseLibrary;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        public bool PrintReceipt { get; set; }
        private Checkout Checkout { get; set; }
        public Loading(Checkout checkout, bool printReceipt)
        {
            InitializeComponent();
            Owner = checkout;
            Checkout = checkout;
            PrintReceipt = printReceipt;

            Loaded += (sender, args) =>
            {
                Main.ActualInvoice.FinalizeInvoice();
                Main.ActualInvoice.Save();

                Task.Run(Print);
            };
        }

        private void NewInvoice_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            Checkout.NewInvoice();
        }

        private void RePrint_OnClick(object sender, RoutedEventArgs e)
        {
            
            ToggleButtons(false);
            ToggleProgressBar(true);

            Task.Run(Print);

        }

        public void ToggleProgressBar(bool toggle)
        {
            SaveLoading.IsIndeterminate = toggle;
        }

        public void Print()
        {
            SaveLoadingText.Content = "Impression...";
            //print
            Main.ActualInvoice.Print(PrintReceipt);

            Dispatcher.Invoke(() => {
                SaveLoadingText.Content = "Terminé";
                ToggleProgressBar(false);
                ToggleButtons(true);
            });
        }

        public void ToggleButtons(bool toggle)
        {
            NewInvoice.IsEnabled = toggle;
            RePrint.IsEnabled = toggle;
        }

    }
}
