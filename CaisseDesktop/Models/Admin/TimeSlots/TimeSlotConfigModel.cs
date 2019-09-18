using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseServer;
using CaisseServer.Events;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static System.Windows.Input.Cursors;

namespace CaisseDesktop.Models.Admin.TimeSlots
{
	public class TimeSlotConfigModel : INotifyPropertyChanged
	{
		private readonly SaveableTimeSlot TimeSlot;
		public Dispatcher Dispatcher { get; set; }

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		private ICommand _editCashierCommand;
		public ICommand EditCashierCommand => _editCashierCommand ?? (_editCashierCommand = new CommandHandler(EditCashier, true));

		private ICommand _editSubstituteCommand;
		public ICommand EditSubstituteCommand => _editSubstituteCommand ?? (_editSubstituteCommand = new CommandHandler(EditSubstitute, true));

		public TimeSlotConfigModel(SaveableTimeSlot timeSlot)
		{
			IsBlank = IsCreating = timeSlot.Blank;
			TimeSlot = timeSlot;
		}
		private bool IsBlank { get; set; }

		public bool IsCreating;

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
			if (Cashier == null)
			{
				Cashier = new SaveableCashier
				{
					Substitute = false,
					Checkout = TimeSlot.Checkout, // Maybe remove this (???)
					LastActivity = DateTime.Now,
					WasHere = false
				};
			}

			//   new CashierManager(this, TimeSlot.Cashier).ShowDialog();
		}

		public void EditSubstitute(object arg)
		{
		}

		public void Save(object arg)
		{
			if (true == true)
			{
				MessageBox.Show(French.Exception_ArgsMissing);
				return;
			}

			Task.Run(Save);
		}

		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Wait; });

			using (var db = new CaisseServerContext())
			{
				// db.Owners.Attach(Checkout.Owner);
				//db.CheckoutTypes.Attach(Checkout.CheckoutType);
				//db.Entry(Checkout).State = IsCreating ? EntityState.Added : EntityState.Modified;
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = null;
				MessageBox.Show(IsCreating ? "La caisse a bien été crée !" : "La caisse a bien été enregistré !");
				IsCreating = false;
			});
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
