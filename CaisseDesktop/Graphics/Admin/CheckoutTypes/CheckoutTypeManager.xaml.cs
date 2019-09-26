using CaisseDesktop.Models.Admin;
using CaisseDesktop.Models.Admin.CheckoutTypes;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Admin.CheckoutTypes
{
	/// <summary>
	/// Interaction logic for CheckoutTypeManager.xaml
	/// </summary>
	public partial class CheckoutTypeManager
	{
		public CheckoutTypeConfigModel Model => DataContext as CheckoutTypeConfigModel;

		public CheckoutTypeManager(EventManagerModel parentModel, SaveableCheckoutType type)
		{
			InitializeComponent();
			DataContext = new CheckoutTypeConfigModel(parentModel, type, Dispatcher);
			Model.CloseAction = Close;
		}

	}
}