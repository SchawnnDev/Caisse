using System.Collections.ObjectModel;
using System.ComponentModel;
using CaisseServer.Events;

namespace CaisseDesktop.Models
{
    public class JourModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableDay> _jours;

        public ObservableCollection<SaveableDay> Jours
        {
            get => _jours;
            set
            {
                if (Equals(value, _jours)) return;

                _jours = value;
                OnPropertyChanged("Jours");
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