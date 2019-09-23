using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Admin.CheckoutTypes;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.IO;
using CaisseDesktop.Utils;
using CaisseLibrary.Enums;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Admin.CheckoutTypes
{
	public class CheckoutTypeConfigModel : INotifyPropertyChanged
	{

		public readonly SaveableCheckoutType CheckoutType;
		public Dispatcher Dispatcher { get; set; }

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		private ICommand _addArticleCommand;
		public ICommand AddArticleCommand => _addArticleCommand ?? (_addArticleCommand = new CommandHandler(AddArticle, true));

		private ICommand _backCommand;
		public ICommand BackCommand => _backCommand ?? (_backCommand = new CommandHandler(Back, true));

		public Action CloseAction { get; set; }
		public static CheckoutTypeManager ParentWindow;

		public CheckoutTypeConfigModel(CheckoutTypeManager parentWindow,SaveableCheckoutType checkoutType)
		{
			ParentWindow = parentWindow;
			IsCreating = checkoutType == null;
			CheckoutType = checkoutType;
			Task.Run(LoadCheckoutNames);
		}

		public bool IsCreating;

		public CheckoutType Type
		{
			get => (CheckoutType)CheckoutType.Type;
			set
			{
				CheckoutType.Type = (int)value;
				OnPropertyChanged();
			}
		}

		public string Name
		{
			get => CheckoutType.Name;
			set
			{
				CheckoutType.Name = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<CheckoutTypeArticle> _articles;
		private ObservableCollection<string> _checkoutNameList;

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

		public ObservableCollection<string> CheckoutNameList
		{
			get => _checkoutNameList;
			set
			{
				_checkoutNameList = value;
				OnPropertyChanged();
			}
		}

		private void AddArticle(object arg)
		{
			if (CheckoutType == null)
			{
				SystemSounds.Beep.Play();
				MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			new ArticleManager(ParentWindow, null).ShowDialog();
		}

		private void Back(object arg)
		{
			CloseAction();
		}

		private void Save(object arg)
		{
			//if (CustomPage.Check(CheckoutTypeName))
			//return;
			/*
			 *
			 * 			if (IsCreating)
			{
				CheckoutType = new SaveableCheckoutType
				{
					Event = Manager.Evenement
				};
			}
			 */

			//	else if (CheckoutType.Name.ToLower().Equals(CheckoutTypeName.Text))
			//	return;


			Task.Run(() => Save());
		}

		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				if (IsCreating)
				{
					if (db.CheckoutTypes.Any(t =>
						t.Event.Id == ParentWindow.Manager.Evenement.Id && t.Name.ToLower().Equals(CheckoutType.Name.ToLower())))
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


				db.Entry(CheckoutType).State = IsCreating ? EntityState.Added : EntityState.Modified;
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{/*
				if (Manager.MasterFrame.ToCustomPage().CustomName.Equals("EventCheckoutTypePage"))
				{
					if (IsCreating) Manager.MasterFrame.ToCustomPage().Add(CheckoutType);
					else Manager.MasterFrame.ToCustomPage().Update(); 
				}
					*/

				Mouse.OverrideCursor = null;
				MessageBox.Show("Le type de caisse a bien été " + (IsCreating ? "crée" : "enregistré") + " !");
			});
		}

		private async void LoadCheckoutNames()
		{
			if (CheckoutType == null) return;

			using (var db = new CaisseServerContext())
			{
				var checkoutNames = await db.Checkouts.Where(t => t.CheckoutType.Id == CheckoutType.Id).Select(t => t.Name).ToListAsync();

				Dispatcher.Invoke(() =>
				{
					CheckoutNameList = new ObservableCollection<string>(checkoutNames);
				});
			}
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

		private void Export(object arg)
		{

			if (arg is SaveableArticle article)
				ExportManager.ExportObjectToJSON($"{BitmapManager.NormalizeFileName(article.Name)}.json", article);
			else
				MessageBox.Show($"L'article n'est pas valide.", "Erreur", MessageBoxButton.OK,
					MessageBoxImage.Error);
		}

		private void Delete(object arg)
		{

			if (arg is SaveableArticle article)
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

			}
			else
			{
				MessageBox.Show($"L'article n'est pas valide.", "Erreur", MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}

		private void Edit(object arg)
		{
			if (arg is SaveableArticle article)
				new ArticleManager(CheckoutTypeConfigModel.ParentWindow, article).ShowDialog();
			else
				MessageBox.Show($"L'article n'est pas valide.", "Erreur", MessageBoxButton.OK,
					MessageBoxImage.Error);
		}

	}
}
