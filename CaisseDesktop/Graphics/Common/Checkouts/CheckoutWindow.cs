using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CaisseServer;

namespace CaisseDesktop.Graphics.Common.Checkouts
{
	public abstract class CheckoutWindow : Window
	{
		public SaveableCheckout SaveableCheckout { get; set; }

		public CheckoutWindow() { }

		public CheckoutWindow(SaveableCheckout checkout)
		{
			SaveableCheckout = checkout;
		}

	}
}
