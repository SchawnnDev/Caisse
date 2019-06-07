using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CaisseDesktop.Graphics.Utils;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Windows
{
	public class CheckoutModel : INotifyPropertyChanged
	{

		private ICommand _articleIncrementCommand;

		public ICommand ArticleIncrementCommand => _articleIncrementCommand ??
												   (_articleIncrementCommand =
													   new CommandHandler(IncrementArticle, true));

		private ICommand _articleDecrementCommand;

		public ICommand ArticleDecrementCommand => _articleDecrementCommand ??
												   (_articleDecrementCommand =
													   new CommandHandler(DecrementArticle, true));

		/**
		 *  Consigns
		 */

		private ICommand _consignIncrementCommand;

		public ICommand ConsignIncrementCommand => _consignIncrementCommand ??
												   (_consignIncrementCommand =
													   new CommandHandler(IncrementConsign, true));

		private ICommand _consignDecrementCommand;

		public ICommand ConsignDecrementCommand => _consignDecrementCommand ??
												   (_consignDecrementCommand =
													   new CommandHandler(DecrementConsign, true));



		//public bool CanExecute => true;

		public decimal Price => Operations.Sum(t => t.FinalPrice);

		public decimal ConsignPrice => ConsignAmount * 1m;

		public decimal FinalPrice => Price + ConsignPrice;

		private int _consignAmount;

		public decimal MoneyToGiveBack => Math.Max(GivenMoney-FinalPrice , 0m);
		
		private decimal _givenMoney;

		public decimal GivenMoney
		{
			get => _givenMoney;
			set
			{
				_givenMoney = value;
				OnPropertyChanged();
				OnPropertyChanged($"MoneyToGiveBack");
			}
		}

		public int ConsignAmount
		{
			get => _consignAmount;
			set
			{
				_consignAmount = value;
				OnPropertyChanged();
			}
		}

		public void NewInvoice()
		{
			OperationList.Clear();
			foreach (var op in Operations)
				op.Amount = 0;
			ConsignAmount = 0;
		}

		public void IncrementConsign(object param)
		{
			ConsignAmount++;

			Update();
		}

		public void DecrementConsign(object param)
		{
			ConsignAmount = Math.Max(ConsignAmount - 1, 0);

			Update();
		}

		public void IncrementArticle(object param)
		{
			if (!(param is CheckoutOperationModel model)) return;

			model.Amount++;

			if (model.NeedsCup) ConsignAmount++;

			Update();
		}

		public void DecrementArticle(object param)
		{
			if (!(param is CheckoutOperationModel model)) return;

			model.Amount = Math.Max(model.Amount - 1, 0);

			if (model.NeedsCup) ConsignAmount = Math.Max(ConsignAmount - 1, 0);

			Update();
		}

		public bool CanDecrementConsign => ConsignAmount > 0;

		public bool IsSomething() => OperationList.Any() && OperationList.Count(t => t.Amount > 0) > 0;

		public Invoice ToInvoice()
		{

			var saveableInvoice = new SaveableInvoice
			{
				Cashier = Main.ActualCashier,
				Date = DateTime.Now,
				GivenMoney = GivenMoney
			};

			var consign = new SaveableConsign
			{
				Invoice = saveableInvoice,
				Amount = ConsignAmount
			};


			return new Invoice(saveableInvoice, consign, OperationList.Select(t=>t.Operation).ToList());

		}

		private void Update()
		{
			OnPropertyChanged($"OperationList");
			OnPropertyChanged($"Price");
			OnPropertyChanged($"ConsignPrice");
			OnPropertyChanged($"FinalPrice");
			OnPropertyChanged($"CanDecrementConsign");
		}

		public ObservableCollection<CheckoutOperationModel> OperationList =>
			new ObservableCollection<CheckoutOperationModel>(Operations.Where(t => t.Amount != 0).ToList());

		private ObservableCollection<CheckoutOperationModel> _operations;

		public ObservableCollection<CheckoutOperationModel> Operations
		{
			get => _operations;
			set
			{
				if (Equals(value, _operations)) return;

				_operations = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}