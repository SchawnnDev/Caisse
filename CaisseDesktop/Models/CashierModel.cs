using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;

namespace CaisseDesktop.Models
{
    public class CashierModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableCashier> _cashiers;

        public ObservableCollection<SaveableCashier> Cashiers
        {
            get => _cashiers;
            set
            {
                if (Equals(value, _cashiers)) return;

                _cashiers = value;
                OnPropertyChanged("Cashiers");
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