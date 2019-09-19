using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Admin.Cashiers;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseServer;
using CaisseServer.Events;
using Cursors = System.Windows.Input.Cursors;

namespace CaisseDesktop.Models.Admin.Cashiers
{
	public class CashierConfigModel : INotifyPropertyChanged
	{
		private readonly SaveableCashier Cashier;
		public Dispatcher Dispatcher { get; set; }

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		private ICommand _generateLoginCommand;
		public ICommand GenerateLoginCommand => _generateLoginCommand ?? (_generateLoginCommand = new CommandHandler(GenerateLogin, true));

		public CashierConfigModel(SaveableCashier cashier)
		{
			IsCreating = cashier == null;
			Cashier = cashier;
		}

		public bool IsCreating;

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

		public void GenerateLogin(object arg)
		{
			throw new NotImplementedException();
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
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

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
