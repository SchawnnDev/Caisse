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
    public class ItemModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableArticle> _items;

        public ObservableCollection<SaveableArticle> Items
        {
            get => _items;
            set
            {
                if (Equals(value, _items)) return;

                _items = value;
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