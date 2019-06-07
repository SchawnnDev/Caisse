using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseDesktop.Models
{
	public class CheckoutOperationRowModel
	{

		public int Amount { get; }
		public decimal FinalPrice { get; }
		public string Name { get; }

		public CheckoutOperationRowModel(string name, int amount, decimal finalPrice)
		{
			Name = name;
			FinalPrice = finalPrice;
			Amount = amount;
		}


	}
}
