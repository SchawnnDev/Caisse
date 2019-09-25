using CaisseDesktop.Models.Admin.CheckoutTypes;
using CaisseServer;
using CaisseServer.Items;
using EventManager = CaisseDesktop.Graphics.Admin.Events.EventManager;

namespace CaisseDesktop.Graphics.Admin.CheckoutTypes
{
	/// <summary>
	/// Interaction logic for CheckoutTypeManager.xaml
	/// </summary>
	public partial class CheckoutTypeManager
	{
		public SaveableCheckoutType CheckoutType { get; set; }
		public EventManager Manager { get; }
		public CheckoutTypeConfigModel Model => DataContext as CheckoutTypeConfigModel;

		public CheckoutTypeManager(EventManager manager, SaveableCheckoutType type)
		{
			InitializeComponent();

			CheckoutType = type;
			Manager = manager;
			DataContext = new CheckoutTypeConfigModel(this, type, Dispatcher);
			Model.CloseAction = Close;

		}

		public void Add(SaveableArticle article)
		{
		//	Model.Articles.Add(new CheckoutTypeArticle(article,this));
		}

		public void Update()
		{
		}

	}
}