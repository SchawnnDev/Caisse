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
    public class ResponsableModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaveableOwner> _responsables;

        public ObservableCollection<SaveableOwner> Responables
        {
            get => _responsables;
            set
            {
                if (Equals(value, _responsables))
                {
                    return;
                }

                _responsables = value;
                OnPropertyChanged("Responables");
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