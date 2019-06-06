using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CaisseDesktop.Graphics.Utils;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Windows
{
	public class CheckoutModel : INotifyPropertyChanged
	{
/*
		private ICommand _clickCommand;
		public ICommand ClickCommand
		{
			get
			{
				return _clickCommand ?? (_clickCommand = new CommandHandler(() => IncrementArticle(), () => CanExecute));
			}
		} */

		public bool CanExecute
		{
			get
			{
				// check if executing is allowed, i.e., validate, check if a process is running, etc. 
				return true;
			}
		}

		public void IncrementArticle(SaveableArticle article)
		{

		}

		private ObservableCollection<SaveableArticle> _articles;

		public ObservableCollection<SaveableArticle> Articles
		{
			get => _articles;
			set
			{
				if (Equals(value, _articles)) return;

				_articles = value;
				OnPropertyChanged("Articles");
			}
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion INotifyPropertyChanged implementation
	}
}