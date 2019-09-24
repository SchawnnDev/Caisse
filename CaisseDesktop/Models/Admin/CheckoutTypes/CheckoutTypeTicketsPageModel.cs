using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.IO;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Admin.CheckoutTypes
{
	public class CheckoutTypeTicketsPageModel : INotifyPropertyChanged
	{
		public readonly CheckoutTypeConfigModel ParentModel;

		private ObservableCollection<CheckoutTypeArticle> _articles;
		public ObservableCollection<CheckoutTypeArticle> Articles
		{
			get => _articles;
			set
			{
				if (Equals(value, _articles)) return;
				_articles = value;
				OnPropertyChanged();
			}
		}


		public CheckoutTypeTicketsPageModel(CheckoutTypeConfigModel parentModel)
		{
			ParentModel = parentModel;
			Task.Run(LoadArticles);
		}

		public void LoadArticles()
		{

			if (ParentModel.IsCreating)
			{
				Articles = new ObservableCollection<CheckoutTypeArticle>();
				return;
			}

			ParentModel.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			ObservableCollection<CheckoutTypeArticle> list;

			using (var db = new CaisseServerContext())
			{
				//		list = new ObservableCollection<CheckoutTypeArticle>(db.Articles.Where(t => t.Type.Id == ParentModel.CheckoutType.Id).OrderBy(t => t.Position).ToList().Select(t => new CheckoutTypeArticle(t, this)).ToList());
			}

			ParentModel.Dispatcher.Invoke(() =>
			{
				//Articles = list;
				Mouse.OverrideCursor = null;
			});
		}



		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}

