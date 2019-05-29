using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;

namespace CaisseDesktop.Models.Windows
{
	public class PaymentMethodManagerModel
	{

		private readonly SaveablePaymentMethod PaymentMethod;

		public PaymentMethodManagerModel(SaveablePaymentMethod method)
		{
			PaymentMethod = method ?? new SaveablePaymentMethod();
		}

		public string Name
		{
			get => PaymentMethod.Name;
			set
			{
				PaymentMethod.Name = value;
				OnPropertyChanged();
			}
		}

		public string Type
		{
			get => PaymentMethod.Type;
			set
			{
				PaymentMethod.Type = value;
				OnPropertyChanged();
			}
		}

		public decimal MinFee
		{
			get => PaymentMethod.MinFee;
			set
			{
				PaymentMethod.MinFee = value;
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
