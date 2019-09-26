using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Admin.Days
{
	public class DayConfigModel : INotifyPropertyChanged
	{
		public readonly SaveableDay Day;
		private DateTime _start;
		private DateTime _end;
		public Dispatcher Dispatcher { get; set; }
		public bool IsCreating;
		private CalendarBlackoutDatesCollection _blackoutDates;
		private readonly EventManagerModel _parentModel;
		public Action CloseAction { get; set; }

		public DayConfigModel(EventManagerModel model, SaveableDay day)
		{
			_parentModel = model;
			IsCreating = day == null;
			Day = day ?? new SaveableDay{Event = model.SaveableEvent};
			Start = model.SaveableEvent.Start;
			End = model.SaveableEvent.End.AddDays(1);

			BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
				model.SaveableEvent.Start.AddDays(-1)));
			BlackoutDates.Add(new CalendarDateRange(model.SaveableEvent.End.AddDays(1),
				DateTime.MaxValue));
		}

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		public DateTime Start
		{
			get => _start;
			set
			{
				_start = value;
				OnPropertyChanged();
			}
		}

		public DateTime End
		{
			get => _end;
			set
			{
				_end = value;
				OnPropertyChanged();
			}
		}

		public CalendarBlackoutDatesCollection BlackoutDates
		{
			get => _blackoutDates;
			set { _blackoutDates = value; OnPropertyChanged(); }
		}

		private void Save(object arg)
		{
		//	var pickerModel = (DayPickerModel)DataContext;

		//	var test = (long)pickerModel.End.ToUnixTimeStamp() - (long)pickerModel.Start.ToUnixTimeStamp();
		/*
			if (test <= 0)
			{
				MessageBox.Show("La fin de l'évenement ne peut pas être avant le début.");
				return;
			}

			if (test > 60 * 60 * 24 &&
				MessageBox.Show("Le jour dure plus de 24h, es-tu sûr de vouloir sauvegarder ?",
					"Jour supérieur à 24h.", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
			{
				return;
			}

			if (CombinedCalendar.SelectedDate == null || EndCombinedCalendar.SelectedDate == null)
			{
				MessageBox.Show("Une erreur est survenue, veuillez réessayer", "Une erreur est survenue",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}*/

			DateTime start = default;// ((DayPickerModel)DataContext).Start;
			DateTime end = default;//((DayPickerModel)DataContext).End;

			using (var db = new CaisseServerContext())
			{
				var days = db.Days.Where(t => t.Event.Id == _parentModel.SaveableEvent.Id).ToList();

				foreach (var day in days)
				{
					// < 0 : date est avant valeur
					// == 0 : date est égale valeur
					// > 0 date est après valeur

					/*
                     *
                     * si value1 est après day1 && day2 est avant value2 || si value1 est avant day1 && day2 est apres value2
                     */

					if ((start.CompareTo(day.Start) <= 0 || day.End.CompareTo(end) >= 0) &&
						(start.CompareTo(day.Start) >= 0 || day.End.CompareTo(end) <= 0) || MessageBox.Show(
							"Le jour chevauche un autre jour déjà enregistré, es-tu sûr de vouloir sauvegarder ?",
							"Jour chevauche un autre.", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
					Task.Run(Save);
					return;
				}

				Task.Run(Save);
			}

			//MessageBox.Show("Sauvegarde...");
		}

		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{

				db.Events.Attach(Day.Event);

				if (IsCreating)
				{
					db.Days.Add(Day);
				}
				else
				{
					//db.Days.Attach(Day);
					db.Days.AddOrUpdate(Day);
				}

				db.SaveChanges();
			}


			Dispatcher.Invoke(() =>
			{
				/*
				if (Manager.MasterFrame.ToCustomPage().CustomName.Equals("EventDayPage"))
				{
					if (New) Manager.MasterFrame.ToCustomPage().Add(Day);
					else Manager.MasterFrame.ToCustomPage().Update();
				} */

				Mouse.OverrideCursor = null;
				MessageBox.Show(IsCreating ? "Le jour a bien été crée !" : "Le jour a bien été enregistré !");

				CloseAction();
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}