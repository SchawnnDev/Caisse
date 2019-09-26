using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Windows;
using CaisseDesktop.Enums;
using CaisseDesktop.Graphics.Print;
using CaisseDesktop.Models.Admin;
using CaisseDesktop.Models.Admin.Checkouts;
using CaisseServer;
using Microsoft.Win32;

namespace CaisseDesktop.Graphics.Admin.Checkouts
{
	/// <summary>
	///     Interaction logic for CheckoutManager.xaml
	/// </summary>
	public partial class CheckoutManager
	{
		public CheckoutManager(EventManagerModel parentModel, SaveableCheckout checkout)
		{
			InitializeComponent();
            //Owner = parentWindow;
			DataContext = new CheckoutManagerModel(parentModel, CheckoutPageType.EditInfos, checkout);
		}


		/*
		public void OnWindowClosing(object sender, CancelEventArgs e)
		{
			if (IsBack || CurrentPage.CanClose()) return;
			e.Cancel = true;
		} */

	}
}