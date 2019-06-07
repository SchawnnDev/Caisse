using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseDesktop.Models.Windows;
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
        private CheckoutModel Model => DataContext as CheckoutModel;

        public SelectPaymentMethod(Checkout checkout, CheckoutModel model)
        {
            InitializeComponent();
            Owner = checkout;
            Checkout = checkout;
            DataContext = model;

			// Blur effect
	        Owner.Effect = new BlurEffect();
			Closed += OnClosed;
        }

	    private void OnClosed(object sender, EventArgs e)
	    {
		    Owner.Effect = null;
	    }

	    private void ValidWithReceipt_OnClick(object sender, RoutedEventArgs e)
        {
            if (Model.FinalPrice - Model.GivenMoney > 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            Close();
            Checkout.OpenLoading(true);
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Model.GivenMoney = 0;
            Close();
        }

        private void ValidWithoutReceipt_OnClick(object sender, RoutedEventArgs e)
        {
            if (Model.FinalPrice - Model.GivenMoney > 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            Close();
            Checkout.OpenLoading(false);
        }


        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            Model.GivenMoney = 0;
        }

        private void Money_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Image img)) return;
            if (!(img.DataContext is string value)) return;
            if (!decimal.TryParse(value, out var result)) return;

            Model.GivenMoney = Math.Min(Model.GivenMoney + result, 10000);
            // todo max configurable => 10.000 € max value
        }

        private void Money50_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Image img)) return;

            Model.GivenMoney = Math.Min(Model.GivenMoney + 0.5m, 10000);
            // todo max configurable => 10.000 € max value
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

            Model.GivenMoney = Math.Min(Model.GivenMoney + result, 10000);
            // todo max configurable => 10.000 € max value

            CustomNumber.Text = "0";
        }

		

	}
}