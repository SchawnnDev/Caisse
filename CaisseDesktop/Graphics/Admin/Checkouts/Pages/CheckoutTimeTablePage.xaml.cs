using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CaisseDesktop.Models.Admin.Checkouts;
using CaisseLibrary.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
	/// <summary>
	///     Interaction logic for CheckoutTimeTablePage.xaml
	/// </summary>
	public partial class CheckoutTimeTablePage
	{

		public CheckoutManagerModel ParentModel { get; set; }

		public CheckoutTimeTableModel Model => DataContext as CheckoutTimeTableModel;

		public CheckoutTimeTablePage(CheckoutManagerModel parentModel)
		{
			InitializeComponent();
			ParentModel = parentModel;
			DataContext = new CheckoutTimeTableModel(parentModel, Dispatcher);
			//Model.Dispatcher = Dispatcher;

		}

		public override string CustomName => "CheckoutTimeTablePage";

		public override void Update()
		{
			Task.Run(Model.UpdateAll);
		}

		public override void Add<T>(T item)
		{
		}

		public override bool CanClose() => true;

		public override bool CanBack() => true;

	}
}