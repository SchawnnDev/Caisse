using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SelectPaymentMethod.xaml
    /// </summary>
    public partial class SelectPaymentMethod
    {
        private Checkout Checkout { get; }
        private readonly Regex OnlyNumbersRegex = new Regex("([0-9])"); //regex that matches allowed text

        public SelectPaymentMethod(Checkout checkout)
        {
            InitializeComponent();
            Owner = checkout;
            Checkout = checkout;
            MoneyToPayLabel.Content = $"A payer : {Main.ActualInvoice.CalculateTotalPrice():F} €";
        }

        private void ValidWithReceipt_OnClick(object sender, RoutedEventArgs e)
        {

            if (Main.ActualInvoice.CalculateTotalPrice() - Main.ActualInvoice.GivenMoney > 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            Close();
            Checkout.OpenLoading(true);
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Main.ActualInvoice.GivenMoney = 0;
            Close();
        }

        private void ValidWithoutReceipt_OnClick(object sender, RoutedEventArgs e)
        {
            if (Main.ActualInvoice.CalculateTotalPrice() - Main.ActualInvoice.GivenMoney > 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            Close();
            Checkout.OpenLoading(false);
        }

        public void UpdateLabels()
        {
            GivenMoneyLabel.Content = $"Reçu : {Main.ActualInvoice.GivenMoney:F} €";
            MoneyGiveBackLabel.Content = $"A rendre : {Main.ActualInvoice.CalculateGivenBackChange():F} €";
        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            if (Main.ActualInvoice.GivenMoney == 0) return;

            Main.ActualInvoice.GivenMoney = 0;
            UpdateLabels();
        }

        private void Money_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Image img)) return;
            if(!(img.DataContext is string value)) return;
            if (!decimal.TryParse(value, out var result)) return;

            Main.ActualInvoice.GivenMoney = Math.Min(Main.ActualInvoice.GivenMoney + result, 10000);
            // todo max configurable => 10.000 € max value
            UpdateLabels();
        }

        // todo avoid insert from letters and write...
        private void CustomAmount_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomNumber.Text) || CustomNumber.Text.Equals("0")) return;

            if (!decimal.TryParse(CustomNumber.Text, out var result))
            {
                CustomNumber.Text = "0";
                return;
            }

            Main.ActualInvoice.GivenMoney = Math.Min(Main.ActualInvoice.GivenMoney + result, 10000);
            // todo max configurable => 10.000 € max value

            CustomNumber.Text = "0";

            UpdateLabels();

        }
    }
}
