using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Windows;
using CaisseDesktop.Utils;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Exceptions;
using CaisseLibrary.Print;
using CaisseServer;
using CaisseServer.Items;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Label = System.Windows.Controls.Label;
using Orientation = System.Windows.Controls.Orientation;
using TextBox = System.Windows.Controls.TextBox;

namespace CaisseDesktop.Graphics.Common.Checkouts
{
	/// <summary>
	///     Interaction logic for ArticleCheckout.xaml
	/// </summary>
	public partial class ArticleCheckout
	{
		private CheckoutModel Model => DataContext as CheckoutModel;

		public ArticleCheckout()
		{
			InitializeComponent();
			DataContext = new CheckoutModel();
			Model.Operations = new ObservableCollection<CheckoutOperationModel>();

			foreach (var article in Main.Articles.OrderBy(t => t.Position).ToList())
			{
				if (!article.Active) continue;
				Model.Operations.Add(new CheckoutOperationModel(article));
			}

		}

		private void SelectPaymentMethod_OnClick(object sender, RoutedEventArgs e)
		{
			if (!Model.IsSomething())
			{
				SystemSounds.Beep.Play();
				return;
			}

			new SelectPaymentMethod(this, Model).ShowDialog();
		}

		public void OpenLoading(bool receiptTicket)
		{
			new Loading(this, Model, receiptTicket).ShowDialog();
		}

		public void NewInvoice()
		{
			Model.NewInvoice();
		}

		private void SwitchUser_OnClick(object sender, RoutedEventArgs e)
		{
			Main.Logout();
			new Login(false).Show();
			Close();
		}

		private void Quit_OnClick(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}