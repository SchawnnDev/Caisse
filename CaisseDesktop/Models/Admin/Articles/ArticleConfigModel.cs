using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using CaisseDesktop.Graphics.Utils;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Admin.Articles
{
	public class ArticleConfigModel : INotifyPropertyChanged
	{

		private readonly string _defaultImagePath =
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images\food.jpg");


		private ICommand _editImageCommand;
		public ICommand EditImageCommand => _editImageCommand ?? (_editImageCommand = new CommandHandler(EditImage, true));

		public readonly SaveableArticle Article;

		public ArticleConfigModel(SaveableArticle article)
		{
			Article = article;
		}


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
			set { Article.Price = value; OnPropertyChanged(); }
		}

		public int MaxSellNumberPerDay
		{
			get => Article.MaxSellNumberPerDay;
			set { Article.MaxSellNumberPerDay = value; OnPropertyChanged(); }
		}

		public Color Color
		{
			get => (Color) ColorConverter.ConvertFromString(Article.Color);
			set { Article.Color = value.ToString(); OnPropertyChanged(); }
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

		private void EditImage(object arg)
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
