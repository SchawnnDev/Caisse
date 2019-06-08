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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseDesktop.Models.Windows;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;

namespace CaisseDesktop.Graphics.Common
{
	/// <summary>
	/// Interaction logic for Loading.xaml
	/// </summary>
	public partial class Loading : Window
	{
		public bool PrintReceipt { get; set; }
		private Checkout Checkout { get; set; }
		private CheckoutModel Model => DataContext as CheckoutModel;
		private Invoice Invoice { get; set; }

		public Loading(Checkout checkout, CheckoutModel model, bool printReceipt)
		{
			InitializeComponent();
			Owner = checkout;
			Checkout = checkout;
			PrintReceipt = printReceipt;
			DataContext = model;
			Invoice = model.ToInvoice();

			Loaded += (sender, args) =>
			{
				Invoice.FinalizeInvoice();
				Invoice.Save();
				Task.Run(() => Print());
			};

			// Blur effect
			Owner.Effect = new BlurEffect();
			Closed += OnClosed;

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

			Task.Run(() => Print());

		}

		public void ToggleProgressBar(bool toggle)
		{
			SaveLoading.IsIndeterminate = toggle;
		}

		public void Print()
		{
			Dispatcher.Invoke(() =>
			{
				SaveLoadingText.Content = "Impression...";
			});

			//print
			Invoice.Print(PrintReceipt);

			Dispatcher.Invoke(() =>
			{
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

		private void OnClosed(object sender, EventArgs e)
		{
			Owner.Effect = null;
		}

	}
}
