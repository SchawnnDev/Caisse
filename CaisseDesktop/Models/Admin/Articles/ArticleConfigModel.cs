using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseDesktop.Models.Admin.CheckoutTypes;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;
using Microsoft.Win32;

namespace CaisseDesktop.Models.Admin.Articles
{
	public class ArticleConfigModel : INotifyPropertyChanged
	{
		private readonly string _defaultImagePath =
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images\food.jpg");

		private ICommand _saveCommand;
		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

		private ICommand _editImageCommand;

		public ICommand EditImageCommand =>
			_editImageCommand ?? (_editImageCommand = new CommandHandler(EditImage, true));

		private ICommand _deleteCommand;
		public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new CommandHandler(Delete, true));

		private ICommand _addCommand;
		public ICommand AddCommand => _addCommand ?? (_addCommand = new CommandHandler(AddMaxSellPerDay, true));

		public readonly SaveableArticle Article;
		private readonly CheckoutTypePage ParentModel;
		public Dispatcher Dispatcher;
		public Action CloseAction { get; set; }

		public ArticleConfigModel(SaveableArticle article, CheckoutTypePage model, Dispatcher dispatcher)
		{
			IsCreating = article == null;
			Article = article ?? new SaveableArticle { Type = model.CheckoutType };
			ParentModel = model;
			Dispatcher = dispatcher;
			Days = new ObservableCollection<SaveableDay>(); // avoid null pointer
			Task.Run(Load);
		}

		public bool IsCreating;


		public string Name
		{
			get => Article.Name;
			set
			{
				Article.Name = value;
				OnPropertyChanged();
			}
		}

		public bool NeedsCup
		{
			get => Article.NeedsCup;
			set
			{
				Article.NeedsCup = value;
				OnPropertyChanged();
			}
		}

		public bool NumberingTracking
		{
			get => Article.NumberingTracking;
			set
			{
				Article.NumberingTracking = value;
				OnPropertyChanged();
			}
		}

		public bool Active
		{
			get => Article.Active;
			set
			{
				Article.Active = value;
				OnPropertyChanged();
			}
		}

		public string ImageSrc
		{
			get => Article == null || string.IsNullOrEmpty(Article.ImageSrc) ? _defaultImagePath : Article.ImageSrc;
			set
			{
				//	CanSave = true;
				Article.ImageSrc = value;
				OnPropertyChanged();
			}
		}

		public decimal Price
		{
			get => Article.Price;
			set
			{
				Article.Price = value;
				OnPropertyChanged();
			}
		}

		public int MaxSellNumberPerDay
		{
			get => Article.MaxSellNumberPerDay;
			set
			{
				Article.MaxSellNumberPerDay = value;
				OnPropertyChanged();
			}
		}

		public Color Color
		{
			get => (Color)ColorConverter.ConvertFromString(Article.Color ?? "White");
			set
			{
				Article.Color = value.ToString();
				OnPropertyChanged();
			}
		}

		public SaveableDay SelectedDay
		{
			get => _selectedDay;
			set { _selectedDay = value; OnPropertyChanged(); }
		}

		public bool BoxActive => Days.Count != 0;

		public string ButtonContent => Days.Count != 0 ? "Ajouter" : "Infos";

		public int MaxSellNumberBox
		{
			get => _maxSellNumberBox;
			set { _maxSellNumberBox = value; OnPropertyChanged(); }
		}

		private ObservableCollection<SaveableArticleMaxSellNumber> _maxSellNumbers;

		public ObservableCollection<SaveableArticleMaxSellNumber> MaxSellNumbers
		{
			get => _maxSellNumbers;
			set
			{
				if (Equals(value, _maxSellNumbers)) return;

				_maxSellNumbers = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<SaveableDay> _days;
		private SaveableDay _selectedDay;
		private int _maxSellNumberBox;

		public ObservableCollection<SaveableDay> Days
		{
			get => _days;
			set
			{
				if (Equals(value, _days)) return;

				_days = value;
				OnPropertyChanged();
				OnPropertyChanged($"ButtonContent");
				OnPropertyChanged($"BoxActive");
			}
		}

		private void Load()
		{
			Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = Cursors.Wait;
			});

			ObservableCollection<SaveableArticleMaxSellNumber> collection;
			ObservableCollection<SaveableDay> days;

			using (var db = new CaisseServerContext())
			{
				collection = IsCreating
					? new ObservableCollection<SaveableArticleMaxSellNumber>()
					: new ObservableCollection<SaveableArticleMaxSellNumber>(db.ArticleMaxSellNumbers
						.Where(e => e.Article.Id == Article.Id).Include(e => e.Day).OrderByDescending(e => e.Day.Start)
						.ToList());

				days = new ObservableCollection<SaveableDay>(db.Days.Where(t => t.Event.Id == ParentModel.CheckoutType.Event.Id).OrderBy(t => t.Start).ToList());
			}

			Dispatcher.Invoke(() =>
			{
				MaxSellNumbers = collection;
				Days = days;
				Mouse.OverrideCursor = null;
			});
		}

		private void Save(object arg)
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				MessageBox.Show(French.Exception_ArgsMissing);
				return;
			}

			Task.Run(Save);
		}

		private void Delete(object arg)
		{

			if (!(arg is SaveableArticleMaxSellNumber number))
				return;

			var result = MessageBox.Show("Es-tu sûr de vouloir supprimer ce nb max de ventes ?",
				"Supprimer un nb max de ventes",
				MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

			if (result != MessageBoxResult.Yes) return;

			if (number.Article != null)
			{
				using (var db = new CaisseServerContext())
				{
					db.ArticleMaxSellNumbers.Attach(number);
					db.ArticleMaxSellNumbers.Remove(number);
					db.SaveChanges();
				}
			}

			MaxSellNumbers.Remove(number);
			Days.Add(number.Day);
			
			OnPropertyChanged($"ButtonContent");
			OnPropertyChanged($"BoxActive");

		}

		private void AddMaxSellPerDay(object arg)
		{
			if (ButtonContent.Equals("Infos"))
			{
				MessageBox.Show("Veuillez ajouter des jours dans la page de gestion de l'évenement.");
				return;
			}

			if (SelectedDay == null)
			{
				MessageBox.Show(French.CaisseException_ErrorOccured);
				return;
			}


			var maxSellPerDay = new SaveableArticleMaxSellNumber
			{
				Day = SelectedDay,
				Amount = MaxSellNumberBox
			};

			MaxSellNumbers.Add(maxSellPerDay);
			Days.Remove(SelectedDay);
			SelectedDay = Days.Count != 0 ? Days[0] : null;

			OnPropertyChanged($"ButtonContent");
			OnPropertyChanged($"BoxActive");

		}


		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				if (IsCreating)
				{
					db.CheckoutTypes.Attach(Article.Type);
					db.Articles.Add(Article);
				}
				else
				{
					db.Articles.Attach(Article);
				}


				db.Entry(Article).State = IsCreating ? EntityState.Added : EntityState.Modified;
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = null;
				MessageBox.Show(IsCreating ? "L'article a bien été crée !" : "L'article a bien été enregistré !");

				if (IsCreating)
					ParentModel.LoadArticles();

				CloseAction();
			});
		}

		private void EditImage(object arg)
		{
			var openFileDialog = new OpenFileDialog
			{
				Title = "Selectionne une image",
				InitialDirectory = string.IsNullOrWhiteSpace(ImageSrc)
					? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
					: ImageSrc,
				Filter = "Fichier images|*.jpg;*.jpeg;*.bmp"
			};

			if (openFileDialog.ShowDialog() != true) return;

			ImageSrc = openFileDialog.FileName;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}