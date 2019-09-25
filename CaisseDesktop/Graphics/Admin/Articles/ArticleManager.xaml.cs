using CaisseDesktop.Models.Admin.Articles;
using CaisseDesktop.Models.Admin.CheckoutTypes;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Admin.Articles
{
	/// <summary>
	/// Interaction logic for ArticleManager.xaml
	/// </summary>
	public partial class ArticleManager
	{
		private ArticleConfigModel Model => DataContext as ArticleConfigModel;

		public ArticleManager(CheckoutTypePage model, SaveableArticle article)
		{
			InitializeComponent();
			DataContext = new ArticleConfigModel(article, model, Dispatcher);
			Model.CloseAction = Close;
		}

	}
}