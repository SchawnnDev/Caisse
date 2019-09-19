using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseDesktop.Graphics.Admin.Cashiers;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Models.Admin.TimeSlots;
using CaisseDesktop.Utils;
using CaisseServer;
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
			Model.CloseAction = Close;
		}

		//private DateTime ModifyTime(DateTime date, DateTime value) => new DateTime(date.Year, date.Month, date.Day, value.Hour, value.Minute, value.Second);

	}
}