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
    public class PaymentMethodModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveablePaymentMethod> _paymentMethods;

        public ObservableCollection<SaveablePaymentMethod> PaymentMethods
        {
            get => _paymentMethods;
            set
            {
                if (Equals(value, _paymentMethods)) return;

                _paymentMethods = value;
                OnPropertyChanged("PaymentMethods");
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
