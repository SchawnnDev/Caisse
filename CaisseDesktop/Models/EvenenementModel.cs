using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Events;

namespace CaisseDesktop.Models
{
    public class EvenementModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableEvent> _evenements;

        public ObservableCollection<SaveableEvent> Evenements
        {
            get => _evenements;
            set
            {
                if (Equals(value, _evenements))
                {
                    return;
                }

                _evenements = value;
                OnPropertyChanged("Evenements");
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