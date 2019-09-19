using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.IO;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Admin.CheckoutTypes
{
	/// <summary>
	/// Interaction logic for CheckoutTypeManager.xaml
	/// </summary>
	public partial class CheckoutTypeManager
	{
		public SaveableCheckoutType CheckoutType { get; set; }
		public EvenementManager Manager { get; }
		private ArticleModel Model => DataContext as ArticleModel;
		private bool New { get; set; }

		public CheckoutTypeManager(EvenementManager manager, SaveableCheckoutType type)
		{
			InitializeComponent();

			CheckoutType = type;
			Manager = manager;
			New = type == null;

			Task.Run(() => Load());

			if (New) return;

			CheckoutTypeName.Text = type.Name;

			Task.Run(() => LoadCheckoutNames());
		}

		public void Add(SaveableArticle article)
		{
			Model.Articles.Add(article);
		}

		public void Update()
		{
			ArticlesGrid.Items.Refresh();
		}

		private void Load()
		{
			Dispatcher.Invoke(() =>
			{
				DataContext = new ArticleModel();
				Mouse.OverrideCursor = Cursors.Wait;
			});

			ObservableCollection<SaveableArticle> collection;

			using (var db = new CaisseServerContext())
			{
				collection =
					New
						? new ObservableCollection<SaveableArticle>()
						: new ObservableCollection<SaveableArticle>(db.Articles.Where(t => t.Type.Id == CheckoutType.Id)
							.OrderBy(t => t.Position).ToList());
			}

			Dispatcher.Invoke(() =>
			{
				Model.Articles = collection;
				Mouse.OverrideCursor = null;

				ArticlesGrid.Loaded += (sender, args) =>
				{
					var i = 0;

					foreach (var article in collection)
					{
						var s = new Style(typeof(DataGridCellsPresenter));
						s.Setters.Add(new Setter(BackgroundProperty,
							new BrushConverter().ConvertFrom(article.Color) as SolidColorBrush));
						var row = (DataGridRow)ArticlesGrid.ItemContainerGenerator.ContainerFromIndex(i++);
						row.Style = s;
					}
				};
			});
		}


		private async void LoadCheckoutNames()
		{
			if (CheckoutType == null) return;

			using (var db = new CaisseServerContext())
			{
				var checkoutNames = await db.Checkouts.Where(t => t.CheckoutType.Id == CheckoutType.Id)
					.Select(t => t.Name).ToListAsync();
				CheckoutNameList.Dispatcher.Invoke(() =>
				{
					foreach (var checkoutName in checkoutNames)
						CheckoutNameList.Items.Add(checkoutName);
				});
			}
		}

		private void Edit_OnClick(object sender, RoutedEventArgs e)
		{
			var btn = sender as Button;

			if (btn?.DataContext is SaveableArticle article)
				new ArticleManager(this, article).ShowDialog();
			else
				MessageBox.Show($"{btn} : l'article n'est pas valide.", "Erreur", MessageBoxButton.OK,
					MessageBoxImage.Error);
		}

		private void Export_OnClick(object sender, RoutedEventArgs e)
		{
			var btn = sender as Button;

			if (btn?.DataContext is SaveableArticle article)
				ExportManager.ExportObjectToJSON($"{BitmapManager.NormalizeFileName(article.Name)}.json", article);
			else
				MessageBox.Show($"{btn} : l'article n'est pas valide.", "Erreur", MessageBoxButton.OK,
					MessageBoxImage.Error);
		}

		private void Delete_OnClick(object sender, RoutedEventArgs e)
		{
			var btn = sender as Button;

			if (btn?.DataContext is SaveableArticle article)
			{
				var result = MessageBox.Show("Es tu sûr de vouloir supprimer cet article ?", "Supprimer un article",
					MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

				if (result != MessageBoxResult.Yes) return;

				using (var db = new CaisseServerContext())
				{
					db.Articles.Attach(article);
					db.Articles.Remove(article);
					db.SaveChanges();
				}

				Model.Articles.Remove(article);
			}
			else
			{
				MessageBox.Show($"{btn} : l'article n'est pas valide.", "Erreur", MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}


		private void AddArticle_OnClick(object sender, RoutedEventArgs e)
		{
			if (CheckoutType == null)
			{
				SystemSounds.Beep.Play();
				MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new ArticleManager(this, null).ShowDialog();
		}

		private void Save_OnClick(object sender, RoutedEventArgs e)
		{
			if (CustomPage.Check(CheckoutTypeName))
				return;

			if (New)
			{
				CheckoutType = new SaveableCheckoutType
				{
					Event = Manager.Evenement
				};
			}
			else if (CheckoutType.Name.ToLower().Equals(CheckoutTypeName.Text))
				return;

			CheckoutType.Name = CheckoutTypeName.Text;

			Task.Run(() => Save());
		}


		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				if (New)
				{
					if (db.CheckoutTypes.Any(t =>
						t.Event.Id == Manager.Evenement.Id && t.Name.ToLower().Equals(CheckoutType.Name.ToLower())))
					{
						Dispatcher.Invoke(() => { return Mouse.OverrideCursor = null; });

						Validations.ShowWarning("Impossible de créer ce type de caisse. Ce nom est déjà utilisé!");
						return;
					}

					db.Events.Attach(CheckoutType.Event);
					db.CheckoutTypes.Add(CheckoutType);
				}
				else
				{
					db.CheckoutTypes.Attach(CheckoutType);
				}


				db.Entry(CheckoutType).State = New ? EntityState.Added : EntityState.Modified;
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				if (Manager.MasterFrame.ToCustomPage().CustomName.Equals("EventCheckoutTypePage"))
				{
					if (New) Manager.MasterFrame.ToCustomPage().Add(CheckoutType);
					else Manager.MasterFrame.ToCustomPage().Update();
				}

				Mouse.OverrideCursor = null;
				MessageBox.Show("Le type de caisse a bien été " + (New ? "crée" : "enregistré") + " !");
			});
		}

		private void Back_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}