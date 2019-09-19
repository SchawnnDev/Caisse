using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Exceptions;
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.TimeSlots;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Admin.Checkouts
{
	public class CheckoutTimeTableModel : INotifyPropertyChanged
	{

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));


		public static CheckoutTimeTablePage ParentWindow;
		public CheckoutTimeTableModel(ObservableCollection<TimeTableDay> timeTableDays, CheckoutTimeTablePage parentWindow)
		{
			TimeTableDays = timeTableDays;
			ParentWindow = parentWindow;
		}

		private ObservableCollection<TimeTableDay> _timeTableDays;


		public ObservableCollection<TimeTableDay> TimeTableDays
		{
			get => _timeTableDays;
			set
			{
				_timeTableDays = value;
				OnPropertyChanged();
			}
		}

		public void Save(object arg)
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class TimeTableDay
	{
		public SaveableDay Day { get; set; }
		public ObservableCollection<TimeTableTimeSlot> TimeSlots { get; set; }
	}

	public class TimeTableTimeSlot
	{
		public string Content { get; set; }
		public SaveableTimeSlot TimeSlot { get; set; }
		private readonly CheckoutTimeTablePage ParentWindow;

		public TimeTableTimeSlot(CheckoutTimeTablePage parentWindow)
		{
			ParentWindow = parentWindow;
		}

		private ICommand _editTimeSlotCommand;
		public ICommand EditTimeSlotCommand => _editTimeSlotCommand ?? (_editTimeSlotCommand = new CommandHandler(EditTimeSlot, true));

		public void EditTimeSlot(object arg)
		{
			if (!(arg is SaveableTimeSlot timeSlot)) throw new CaisseException(French.CaisseException_ErrorOccured);
			new TimeSlotManager(ParentWindow.ParentWindow, timeSlot).ShowDialog();
		}

	}

}