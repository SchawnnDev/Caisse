using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Models
{
    public class CaisseModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableCheckout> _caisses;

        public ObservableCollection<SaveableCheckout> Caisses
        {
            get => _caisses;
            set
            {
                if (Equals(value, _caisses))
                {
                    return;
                }

                _caisses = value;
                OnPropertyChanged("Caisses");
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
