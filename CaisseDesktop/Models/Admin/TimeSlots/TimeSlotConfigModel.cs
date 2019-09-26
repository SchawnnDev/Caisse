
using CaisseDesktop.Graphics.Admin.Cashiers;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseServer;
using CaisseServer.Events;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CaisseDesktop.Models.Admin.TimeSlots
{
	public class TimeSlotConfigModel : INotifyPropertyChanged
	{
		public readonly SaveableTimeSlot TimeSlot;
		public Dispatcher Dispatcher { get; set; }

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		private ICommand _deleteCommand;
		public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new CommandHandler(Delete, true));

		private ICommand _editCashierCommand;
		public ICommand EditCashierCommand => _editCashierCommand ?? (_editCashierCommand = new CommandHandler(EditCashier, true));

		private ICommand _editSubstituteCommand;
		public ICommand EditSubstituteCommand => _editSubstituteCommand ?? (_editSubstituteCommand = new CommandHandler(EditSubstitute, true));

		public Action CloseAction { get; set; }

		public TimeSlotConfigModel(SaveableTimeSlot timeSlot)
		{
			IsBlank = IsCreating = timeSlot.Blank;
			TimeSlot = timeSlot;
		}
		private bool IsBlank { get; set; }

		private bool _isCreating;

		public bool IsCreating
		{
			get => _isCreating;
			set
			{
				_isCreating = value;
				OnPropertyChanged();
			}
		}

		public DateTime Start
		{
			get => TimeSlot.Start;
			set
			{
				TimeSlot.Start = value;
				OnPropertyChanged();
			}
		}

		public DateTime End
		{
			get => TimeSlot.End;
			set
			{
				TimeSlot.End = value;
				OnPropertyChanged();
			}
		}

		public SaveableCashier Cashier
		{
			get => TimeSlot.Cashier;
			set
			{
				TimeSlot.Cashier = value;
				IsBlank = false;
				OnPropertyChanged();
				OnPropertyChanged($"CashierName");
			}
		}

		public SaveableCashier Substitute
		{
			get => TimeSlot.Substitute;
			set
			{
				TimeSlot.Substitute = value;
				IsBlank = false;
				OnPropertyChanged();
				OnPropertyChanged($"SubstituteName");
			}
		}

		public bool SubstituteActive
		{
			get => TimeSlot.SubstituteActive;
			set
			{
				TimeSlot.SubstituteActive = value;
				OnPropertyChanged();
			}
		}

		public string CashierName => IsBlank || TimeSlot.Cashier == null ? "Créer" : TimeSlot.Cashier.GetFullName();

		public string SubstituteName => IsBlank || TimeSlot.Substitute == null ? "Créer" : TimeSlot.Substitute.GetFullName();

		public bool Pause
		{
			get => TimeSlot.Pause;
			set
			{
				TimeSlot.Pause = value;
				OnPropertyChanged();
			}
		}

		public void EditCashier(object arg)
		{
			new CashierManager(this, true).ShowDialog();
		}

		public void EditSubstitute(object arg)
		{
			new CashierManager(this, false).ShowDialog();
		}

		private void NormalizeHours()
		{
			Start = ModifyTime(TimeSlot.Day.Start, Start);
			End = ModifyTime(TimeSlot.Day.End, End);
		}

		public void Delete(object arg)
		{

			var result = MessageBox.Show("Es tu sûr de vouloir supprimer ce créneau horaire?", "Supprimer un créneau horaire",
				MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

			if (result != MessageBoxResult.Yes) return;


			Task.Run(() =>
			{

				using (var db = new CaisseServerContext())
				{
					if (db.TimeSlots.Any(t => t.Id == TimeSlot.Id)) {
						db.TimeSlots.Attach(TimeSlot);
						db.TimeSlots.Remove(TimeSlot);
					}
					db.SaveChangesAsync();
				}

				Dispatcher.Invoke(() =>
				{
					CloseAction();
				});
			});


		}

		public void Save(object arg)
		{

			NormalizeHours();

			if (TimeSlot.End.CompareTo(Start) < 0)
			{
				MessageBox.Show(French.TimeSlotConfigModel_Save_Hour);
				return;
			}

			if (TimeSlot.Start.CompareTo(End) > 0)
			{
				MessageBox.Show(French.TimeSlotConfigModel_Save_Hour2);
				return;
			}

			if(!Pause && (Cashier == null || SubstituteActive && Substitute == null))
			{
				MessageBox.Show(French.Exception_ArgsMissing);
				return;
			}

			Task.Run(Save);
		}

		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				if (Cashier != null)
				{
					if (db.Cashiers.Any(t => t.Id == TimeSlot.Cashier.Id))
					{
						db.Cashiers.Attach(TimeSlot.Cashier);
						db.Entry(TimeSlot.Cashier).State = EntityState.Modified;
					}
					else
					{
						db.Checkouts.Attach(TimeSlot.Cashier.Checkout); // Attach checkouts (maybe remove???)
						db.Entry(TimeSlot.Cashier).State = EntityState.Added;
					}
				}

				if (Substitute != null)
				{
					if (db.Cashiers.Any(t => t.Id == TimeSlot.Substitute.Id))
					{
						db.Cashiers.Attach(TimeSlot.Substitute);
						db.Entry(TimeSlot.Substitute).State = EntityState.Modified;
					}
					else
					{
						db.Checkouts.Attach(TimeSlot.Substitute.Checkout); // Attach checkouts (maybe remove???)
						db.Entry(TimeSlot.Substitute).State = EntityState.Added;
					}
				}

				if (TimeSlot.Blank)
				{
					db.Checkouts.Attach(TimeSlot.Checkout);
					db.Days.Attach(TimeSlot.Day);
					// Attach etc.
					db.TimeSlots.Add(TimeSlot);
				}
				else
				{
					db.TimeSlots.Attach(TimeSlot);
				}

				db.Entry(TimeSlot).State = TimeSlot.Blank ? EntityState.Added : EntityState.Modified;

				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = null;
				MessageBox.Show(TimeSlot.Blank ? "Le créneau horaire a bien été crée !" : "Le créneau horaire a bien été enregistré !");
				CloseAction();
			});
		}

		private DateTime ModifyTime(DateTime date, DateTime value) => new DateTime(date.Year, date.Month, date.Day, value.Hour, value.Minute, value.Second);

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
