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
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Windows
{
	public class CheckoutModel : INotifyPropertyChanged
	{

		private ICommand _articleIncrementCommand;
		public ICommand ArticleIncrementCommand => _articleIncrementCommand ?? (_articleIncrementCommand = new CommandHandler(IncrementArticle, true));

		private ICommand _articleDecrementCommand;
		public ICommand ArticleDecrementCommand => _articleDecrementCommand ?? (_articleDecrementCommand = new CommandHandler(DecrementArticle, true));

		public bool CanExecute
		{
			get
			{
				// check if executing is allowed, i.e., validate, check if a process is running, etc. 
				return true;
			}
		}

		public void IncrementArticle(object article)
		{
			if (!(article is SaveableArticle saveableArticle)) return;

			if (Operations.Any(t => t.Item.Id == saveableArticle.Id))
			{
				Operations.Single(t => t.Item.Id == saveableArticle.Id).Amount++;
			}
			else
			{
				Operations.Add(new SaveableOperation { Amount = 1, Invoice = Main.ActualInvoice.SaveableInvoice, Item = saveableArticle });
			}

			OnPropertyChanged($"Operations");

		}

		public void DecrementArticle(object article)
		{
			if (!(article is SaveableArticle saveableArticle)) return;

			if (Operations.Any(t => t.Item.Id == saveableArticle.Id))
				Operations.Single(t => t.Item.Id == saveableArticle.Id).Amount--;

			OnPropertyChanged($"Operations");

		}

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