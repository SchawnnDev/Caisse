using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models
{
    public class ArticleModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableArticle> _articles;

        public ObservableCollection<SaveableArticle> Articles
        {
            get => _articles;
            set
            {
                if (Equals(value, _articles)) return;

                _articles = value;
                OnPropertyChanged("Articles");
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion INotifyPropertyChanged implementation
    }
}