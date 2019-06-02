using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Windows
{
	public class TimeSlotManagerModel
	{

		private SaveableTimeSlot TimeSlot { get; }

		public TimeSlotManagerModel(SaveableTimeSlot timeSlot)
		{
			TimeSlot = timeSlot;
		}

		public bool IsPause
		{
			get => TimeSlot.Pause;
			set
			{
				TimeSlot.Pause = value;
				OnPropertyChanged();
			}
		}

		public DateTime StartTime
		{
			get => TimeSlot.Start;
			set
			{
				TimeSlot.Start = ModifyTime(value);
				OnPropertyChanged();
			}
		}

		public DateTime EndTime
		{
			get => TimeSlot.End;
			set
			{
				TimeSlot.End = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private DateTime ModifyTime(DateTime value) => new DateTime(TimeSlot.Day.Start.Year, TimeSlot.Day.Start.Month, TimeSlot.Day.Start.Day, value.Hour, value.Minute, value.Second);

	}
}
