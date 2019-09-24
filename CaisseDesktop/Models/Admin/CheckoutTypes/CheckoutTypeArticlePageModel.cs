using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
	public class CheckoutTypeArticlePageModel : INotifyPropertyChanged
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


		public CheckoutTypeArticlePageModel(CheckoutTypeConfigModel parentModel)
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
				list = new ObservableCollection<CheckoutTypeArticle>(db.Articles.Where(t => t.Type.Id == ParentModel.CheckoutType.Id).OrderBy(t => t.Position).ToList().Select(t => new CheckoutTypeArticle(t, this)).ToList());
			}

			ParentModel.Dispatcher.Invoke(() =>
			{
				Articles = list;
				Mouse.OverrideCursor = null;
			});
		}



		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class CheckoutTypeArticle
	{
		public SaveableArticle Article { get; set; }

		private ICommand _deleteCommand;
		public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new CommandHandler(Delete, true));

		private ICommand _editCommand;
		public ICommand EditCommand => _editCommand ?? (_editCommand = new CommandHandler(Edit, true));

		private ICommand _exportCommand;
		public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new CommandHandler(Export, true));

		private readonly CheckoutTypeArticlePageModel _parentModel;

		public CheckoutTypeArticle(SaveableArticle article, CheckoutTypeArticlePageModel parentModel)
		{
			Article = article;
			_parentModel = parentModel;
		}

		private void Export(object arg)
		{
			ExportManager.ExportObjectToJSON($"{BitmapManager.NormalizeFileName(Article.Name)}.json", Article);
		}

		private void Delete(object arg)
		{
			var result = MessageBox.Show("Es tu sûr de vouloir supprimer cet article ?", "Supprimer un article",
				MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

			if (result != MessageBoxResult.Yes) return;

			using (var db = new CaisseServerContext())
			{
				db.Articles.Attach(Article);
				db.Articles.Remove(Article);
				db.SaveChanges();
			}

			_parentModel.Articles.Remove(this);
		}

		private void Edit(object arg)
		{
			new ArticleManager(_parentModel.ParentModel.ParentWindow, Article).ShowDialog();
		}

	}
}
