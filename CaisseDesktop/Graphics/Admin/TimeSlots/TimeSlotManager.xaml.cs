using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Models.Admin.TimeSlots;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.TimeSlots
{
	/// <summary>
	/// Interaction logic for TimeSlotManager.xaml
	/// </summary>
	public partial class TimeSlotManager
	{

		public TimeSlotConfigModel Model => DataContext as TimeSlotConfigModel;

		public TimeSlotManager(CheckoutManager parentWindow, SaveableTimeSlot timeSlot)
		{
			InitializeComponent();
			Owner = parentWindow;
			DataContext = new TimeSlotConfigModel(timeSlot);
			Model.Dispatcher = Dispatcher;
			Model.CloseAction = () =>
			{
				parentWindow.CurrentPage.Update();
				Close();
			};
		}

	}
}