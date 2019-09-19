using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Media;
using CaisseDesktop.Admin;
using CaisseDesktop.Graphics.Admin.TimeSlots;
using CaisseDesktop.Models.Admin.Cashiers;
using CaisseDesktop.Models.Admin.TimeSlots;
using CaisseDesktop.Utils;
using CaisseLibrary.Concrete.Owners;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Cashiers
{
	/// <summary>
	///     Interaction logic for OwnerManager.xaml
	/// </summary>
	public partial class CashierManager
	{

		public CashierConfigModel Model => DataContext as CashierConfigModel;

		public CashierManager(TimeSlotConfigModel model, bool cashier)
		{
			InitializeComponent();
			DataContext = new CashierConfigModel(model, cashier);
			Model.CloseAction = Close;
			//Owner = parentWindow;
		}

	}
}