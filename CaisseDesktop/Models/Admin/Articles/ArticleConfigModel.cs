using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Lang;
using CaisseDesktop.Models.Admin.CheckoutTypes;
using CaisseServer;
using CaisseServer.Items;
using static System.Windows.Input.Cursors;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

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

        public readonly SaveableArticle Article;
        private readonly CheckoutTypePage ParentModel;
        public Dispatcher Dispatcher;
        public Action CloseAction { get; set; }

        public ArticleConfigModel(SaveableArticle article, CheckoutTypePage model)
        {
            IsCreating = article == null;
            Article = article ?? new SaveableArticle {Type = model.CheckoutType};
            ParentModel = model;
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
            get => (Color) ColorConverter.ConvertFromString(Article.Color ?? "White");
            set
            {
                Article.Color = value.ToString();
                OnPropertyChanged();
            }
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

        private void Save(object arg)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show(French.Exception_ArgsMissing);
                return;
            }

            Task.Run(Save);
        }


        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Wait; });

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