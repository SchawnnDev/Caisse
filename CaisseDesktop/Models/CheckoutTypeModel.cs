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
    public class CheckoutTypeModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableCheckoutType> _checkoutTypes;

        public ObservableCollection<SaveableCheckoutType> CheckoutTypes
        {
            get => _checkoutTypes;
            set
            {
                if (Equals(value, _checkoutTypes))
                {
                    return;
                }

                _checkoutTypes = value;
                OnPropertyChanged("CheckoutTypes");
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