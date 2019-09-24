using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Utils;
using CaisseLibrary.Data;
using CaisseServer;

namespace CaisseDesktop.Models.Admin.CheckoutTypes
{
    public abstract class CheckoutTypePage : INotifyPropertyChanged
    {

        public SaveableCheckoutType CheckoutType { get; set; }

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

        private ICommand _addArticleCommand;
        public ICommand AddArticleCommand => _addArticleCommand ?? (_addArticleCommand = new CommandHandler(AddArticle, true));

        private void AddArticle(object arg)
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

        protected CheckoutTypePage(SaveableCheckoutType type)
        {
            CheckoutType = type;
        }

        public abstract void LoadArticles();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
