using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Windows
{
	public class DisplayInvoiceModel
	{
		public SaveableInvoice Invoice { get; }

		private SaveableConsign Consign { get; set; }

		private decimal _finalPrice;

		public decimal FinalPrice
		{
			get => _finalPrice;
			set
			{
				_finalPrice = value;
				OnPropertyChanged();
			}
		}

		public DisplayInvoiceModel(SaveableInvoice invoice)
		{
			Invoice = invoice;
			LoadOperations();
		}

		private void LoadOperations()
		{
			using (var db = new CaisseServerContext())
			{

				Operations = new ObservableCollection<Operation>();

				foreach (var operation in db.Operations.Where(t => t.Invoice.Id == Invoice.Id).Include(t => t.Item).ToList())
				{
					Operations.Add(new Operation
					{
						Name = operation.Item.Name,
						Price = operation.FinalPrice,
						Quantity = operation.Amount
					});
				}

				
				if (db.Consigns.Any(t => t.Invoice.Id == Invoice.Id)) { 
					Consign = db.Consigns.Single(t => t.Invoice.Id == Invoice.Id);
					FinalPrice += Consign.Amount; /* todo: different consigns */
					Operations.Add(new Operation
					{
						Name = "Consigne",
						Price = Consign.Amount,
						Quantity = Consign.Amount
					});
				}
			}

			FinalPrice += Operations.Sum(t => t.Price);
		}

		private ObservableCollection<Operation> _operations;

		public ObservableCollection<Operation> Operations
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

	public struct Operation
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		
	}
}