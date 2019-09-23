using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using CaisseDesktop.Exceptions;
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.TimeSlots;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseLibrary.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Admin.Checkouts
{
	public class CheckoutTimeTableModel : INotifyPropertyChanged
	{

		public static CheckoutTimeTablePage ParentWindow;
		public CheckoutTimeTableModel(CheckoutTimeTablePage parentWindow)
		{
			ParentWindow = parentWindow;
			Task.Run(UpdateAll);
		}

		private async Task<Dictionary<SaveableDay, List<SaveableTimeSlot>>> LoadTimeSlotsFromDb()
		{
			var dic = new Dictionary<SaveableDay, List<SaveableTimeSlot>>();

			using (var db = new CaisseServerContext())
			{
				var days = await db.Days.Where(t => t.Event.Id == ParentWindow.ParentWindow.ParentWindow.Evenement.Id)
					.OrderBy(t => t.Start).ToListAsync();
				foreach (var day in days)
				{

					var timeSlots = await db.TimeSlots.Where(t => t.Day.Id == day.Id).OrderBy(t => t.Start)
						.Include(t => t.Cashier).Include(t => t.Substitute).Include(t => t.Checkout).Include(t => t.Day)
						.ToListAsync();

					timeSlots.AddRange(GenerateBlankSlots(day, timeSlots));

					dic.Add(day, timeSlots);

				}
			}

			return dic;
		}

		private void Update(Dictionary<SaveableDay, List<SaveableTimeSlot>> timeSlots)
		{

			var list = new ObservableCollection<TimeTableDay>();

			foreach (var day in timeSlots)
			{

				var timeTableDay = new TimeTableDay
				{
					Day = day.Key,
					TimeSlots = new ObservableCollection<TimeTableTimeSlot>()
				};

				foreach (var tableTimeSlot in day.Value)
				{
					var timeSlot = new TimeTableTimeSlot(ParentWindow)
					{
						TimeSlot = tableTimeSlot
					};

					timeTableDay.TimeSlots.Add(timeSlot);

				}

				timeTableDay.TimeSlots = new ObservableCollection<TimeTableTimeSlot>(timeTableDay.TimeSlots.OrderBy(t => t.TimeSlot.Start).ToList());
				list.Add(timeTableDay);

			}

			TimeTableDays = list;

		}

		private List<SaveableTimeSlot> GenerateBlankSlots(SaveableDay day, List<SaveableTimeSlot> taken)
		{
			var blankSlots = new List<SaveableTimeSlot>();

			var checkout = ParentWindow.ParentWindow.Checkout;
			var dayStartHour = day.Start.Hour;
			var dayEndHour = day.End.Hour;
			var dayStartMinute = day.Start.Minute;
			var dayEndMinute = day.End.Minute;

			if (taken.Count == 0)
			{
				blankSlots.Add(new SaveableTimeSlot
				{
					Blank = true,
					Start = day.Start,
					End = day.End,
					Day = day,
					Checkout = checkout
				});
				return blankSlots;
			}

			var min = taken.Min(t => t.Start);

			if (min.Hour == dayStartHour && min.Minute != dayStartMinute || min.Hour != dayStartHour)
			{
				blankSlots.Add(new SaveableTimeSlot
				{
					Start = day.Start,
					End = min
				});
			}

			for (var i = 1; i < taken.Count; i++)
			{
				var t1 = taken[i - 1].End;
				var t2 = taken[i].Start;

				if ((t1.Hour != t2.Hour || t1.Minute == t2.Hour) && t1.Hour == t2.Hour) continue;
				if (t2.ToUnixTimeStamp() - t1.ToUnixTimeStamp() <= 5)
					continue;

				blankSlots.Add(new SaveableTimeSlot
				{
					Start = t1,
					End = t2
				});
			}


			var max = taken.Max(t => t.End);

			if (max.Hour == dayEndHour && max.Minute != dayEndMinute || max.Hour != dayEndHour)
			{
				blankSlots.Add(new SaveableTimeSlot
				{
					Start = max,
					End = day.End
				});
			}

			blankSlots.Select(t =>
			{
				t.Blank = true;
				t.Checkout = checkout;
				t.Day = day;
				return t;
			}).ToList();

			return blankSlots;
		}

		private string DateToHour(DateTime date) => $"{date.Hour}h{(date.Minute < 10 ? $"0{date.Minute}" : date.Minute.ToString())}";

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

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public async void UpdateAll()
		{
			ParentWindow.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
			Update(await LoadTimeSlotsFromDb());
			ParentWindow.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
		}
	}

	public class TimeTableDay
	{
		public SaveableDay Day { get; set; }
		public ObservableCollection<TimeTableTimeSlot> TimeSlots { get; set; }
	}

	public class TimeTableTimeSlot
	{
		public SaveableTimeSlot TimeSlot { get; set; }
		private readonly CheckoutTimeTablePage ParentWindow;

		public TimeTableTimeSlot(CheckoutTimeTablePage parentWindow)
		{
			ParentWindow = parentWindow;
		}

		private ICommand _editTimeSlotCommand;
		public ICommand EditTimeSlotCommand => _editTimeSlotCommand ?? (_editTimeSlotCommand = new CommandHandler(EditTimeSlot, true));

		public SolidColorBrush Color
		{
			get
			{
				switch (GetStatus())
				{
					case TimeSlotStatus.Available:
						return new SolidColorBrush(System.Windows.Media.Color.FromRgb(190, 215, 209));
					case TimeSlotStatus.Pause:
						return new SolidColorBrush(System.Windows.Media.Color.FromRgb(247, 235, 195));
					case TimeSlotStatus.TakenByCashier:
						return new SolidColorBrush(System.Windows.Media.Color.FromRgb(121, 159, 203));
					case TimeSlotStatus.TakenBySubstitute:
						return new SolidColorBrush(System.Windows.Media.Color.FromRgb(249, 212, 148));
					default:
						return new SolidColorBrush(System.Windows.Media.Color.FromRgb(249, 102, 94));
				}
			}
		}

		public string Content
		{
			get
			{
				switch (GetStatus())
				{
					case TimeSlotStatus.Available:
						return "Clique ici pour assigner la case.";
					case TimeSlotStatus.Pause:
						return "Pause";
					case TimeSlotStatus.TakenByCashier:
						return TimeSlot.Cashier.GetFullName();
					case TimeSlotStatus.TakenBySubstitute:
						return $"{TimeSlot.Substitute.GetFullName()} (Remplaçant)";
					default:
						return "exception";
				}
			}
		}

		public void EditTimeSlot(object arg)
		{
			if (!(arg is SaveableTimeSlot timeSlot)) throw new CaisseException(French.CaisseException_ErrorOccured);
			new TimeSlotManager(ParentWindow.ParentWindow, timeSlot).ShowDialog();
		}

		public TimeSlotStatus GetStatus()
		{
			if (TimeSlot.Blank) return TimeSlotStatus.Available;
			if (TimeSlot.Pause) return TimeSlotStatus.Pause;
			if (TimeSlot.SubstituteActive && TimeSlot.Substitute != null) return TimeSlotStatus.TakenBySubstitute;
			return TimeSlot.Cashier != null ? TimeSlotStatus.TakenByCashier : default;
		}

	}

	public enum TimeSlotStatus
	{
		Available,
		Pause,
		TakenByCashier,
		TakenBySubstitute
	}

}