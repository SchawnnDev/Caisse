using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Admin.Cashiers;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseDesktop.Models.Admin.TimeSlots;
using CaisseLibrary.Concrete.Owners;
using CaisseServer;
using CaisseServer.Events;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Cursors = System.Windows.Input.Cursors;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CaisseDesktop.Models.Admin.Cashiers
{
	public class CashierConfigModel : INotifyPropertyChanged
	{
		public Dispatcher Dispatcher { get; set; }

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		private ICommand _deleteCommand;
		public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new CommandHandler(Delete, true));

		private ICommand _generateLoginCommand;
		public ICommand GenerateLoginCommand => _generateLoginCommand ?? (_generateLoginCommand = new CommandHandler(GenerateLogin, true));

		public bool IsCreating { get; set; }

		private readonly bool IsCashier;
		private TimeSlotConfigModel Model { get; }
		public Action CloseAction { get; set; }
		private readonly SaveableCashier Cashier;

		public CashierConfigModel(TimeSlotConfigModel model, bool cashier)
		{
			Cashier = cashier ? model.Cashier : model.Substitute;
			IsCreating = Cashier == null;
			IsCashier = cashier;
			Model = model;

			if (Cashier == null)
				Cashier = CreateNewCashier(cashier);
		}

		public SaveableCashier CreateNewCashier(bool cashier)
		{
			return new SaveableCashier
			{
				Substitute = !cashier,
				Checkout = Model.TimeSlot.Checkout, // Maybe remove this (???)
				LastActivity = DateTime.Now,
				WasHere = false
			};
		}

		public string FirstName
		{
			get => Cashier.FirstName;
			set
			{
				Cashier.FirstName = value;
				OnPropertyChanged();
			}
		}

		public string Name
		{
			get => Cashier.Name;
			set
			{
				Cashier.Name = value;
				OnPropertyChanged();
			}
		}

		public string Login
		{
			get => Cashier.Login;
			set
			{
				Cashier.Login = value;
				OnPropertyChanged();
			}
		}

		public DateTime LastActivity
		{
			get => Cashier.LastActivity;
			set
			{
				Cashier.LastActivity = value;
				OnPropertyChanged();
			}
		}

		public bool WasHere
		{
			get => Cashier.WasHere;
			set
			{
				Cashier.WasHere = value;
				OnPropertyChanged();
			}
		}

		public void GenerateLogin(object arg)
		{
			//if (SessionAdmin.HasNotPermission("owners.login.gen"))
			//	return;

			using (var db = new CaisseServerContext())
			{// possible characters : 123456789ABCXYZ/*- and length=7
				Login = new CashierPassword().GenerateNoDuplicate(7, db.Cashiers.Select(t => t.Login).ToList());
			}
		}

		public void Delete(object arg)
		{
			var result = System.Windows.MessageBox.Show("Es tu sûr de vouloir supprimer ce caissier ?", "Supprimer un caissier",
				MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

			if (result != MessageBoxResult.Yes) return;

			var cashier = Cashier;

			if (IsCashier) Model.Cashier = null;
			else Model.Substitute = null;

			Task.Run(() =>
			{
				using (var db = new CaisseServerContext())
				{
					if (db.Cashiers.Any(t => t.Id == cashier.Id))
						db.Cashiers.Remove(cashier);
					db.SaveChangesAsync();
				}
			});

		}

		public void Save(object arg)
		{
			if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(Name) ||
			    string.IsNullOrWhiteSpace(Login))
			{
				MessageBox.Show(French.Exception_ArgsMissing);
				return;
			}

			if (IsCashier) Model.Cashier = Cashier;
			else Model.Substitute = Cashier;

			// direct close of dialog
			CloseAction();

		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
