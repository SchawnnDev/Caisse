using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.IO;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Admin.CheckoutTypes;
using CaisseDesktop.Utils;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Admin.CheckoutTypes
{
	/// <summary>
	/// Interaction logic for CheckoutTypeManager.xaml
	/// </summary>
	public partial class CheckoutTypeManager
	{
		public SaveableCheckoutType CheckoutType { get; set; }
		public EvenementManager Manager { get; }
		private CheckoutTypeConfigModel Model => DataContext as CheckoutTypeConfigModel;
		private bool New { get; set; }

		public CheckoutTypeManager(EvenementManager manager, SaveableCheckoutType type)
		{
			InitializeComponent();

			CheckoutType = type;
			Manager = manager;
			New = type == null;

			DataContext = new CheckoutTypeConfigModel(this,type);
			Model.Dispatcher = Dispatcher;
			Model.CloseAction = Close;

		}

		public void Add(SaveableArticle article)
		{
		}

		public void Update()
		{
		}

	}
}