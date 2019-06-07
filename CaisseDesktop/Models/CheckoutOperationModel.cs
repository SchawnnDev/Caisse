using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CaisseDesktop.Models.Windows;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models
{
	public class CheckoutOperationModel : INotifyPropertyChanged
	{

		public SaveableOperation Operation;

		public CheckoutOperationModel(SaveableArticle article)
		{
			Operation = new SaveableOperation { Item = article };
		}

		public int Amount
		{
			get => Operation.Amount;
			set
			{
				Operation.Amount = value;
				OnPropertyChanged();
                OnPropertyChanged($"CanDecrement");
			}
		}

        public int MaxSellNumberPerDay
		{
			get => Operation.Item.MaxSellNumberPerDay;
			set
			{
				Operation.Item.MaxSellNumberPerDay = value;
				OnPropertyChanged();
			}
		}

        public bool CanDecrement => Amount > 0;

        public string Name => Operation.Item.Name;

		public string ImageSrc => Operation.Item.ImageSrc;

		public decimal Price => Operation.Item.Price;

		public bool NeedsCup => Operation.Item.NeedsCup;

		public bool Active => Operation.Item.Active;

        public decimal FinalPrice => Operation.Item.Price * Amount;


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}
