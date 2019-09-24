using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaisseDesktop.Models.Admin.CheckoutTypes;

namespace CaisseDesktop.Graphics.Admin.CheckoutTypes.Pages
{
	/// <summary>
	/// Interaction logic for CheckoutTypeArticlePage.xaml
	/// </summary>
	public partial class CheckoutTypeArticlePage
	{

		private CheckoutTypeArticlePageModel Model => DataContext as CheckoutTypeArticlePageModel;

		public CheckoutTypeArticlePage(CheckoutTypeConfigModel model)
		{
			InitializeComponent();
			DataContext = new CheckoutTypeArticlePageModel(model);
		}

		public override string CustomName => "Liste des articles";
		public override void Update()
		{
		}

		public override void Add<T>(T item)
		{
		}

		public override bool CanClose()
		{
			return true;
		}

		public override bool CanBack()
		{
			return true;
		}
	}
}
