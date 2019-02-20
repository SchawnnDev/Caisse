using System.Collections.ObjectModel;
using System.ComponentModel;
using CaisseServer.Items;

namespace CaisseDesktop.Models
{
    public class MaxSellNumberModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableArticleMaxSellNumber> _maxSellNumbers;

        public ObservableCollection<SaveableArticleMaxSellNumber> MaxSellNumbers
        {
            get => _maxSellNumbers;
            set
            {
                if (Equals(value, _maxSellNumbers)) return;

                _maxSellNumbers = value;
                OnPropertyChanged("MaxSellNumbers");
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
