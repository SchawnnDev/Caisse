using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CaisseLibrary.Data;

namespace CaisseDesktop.Models
{
    public class PermissionModel : INotifyPropertyChanged
    {
        private ObservableCollection<Permission> _permissions;

        public ObservableCollection<Permission> Permissions
        {
            get => _permissions;
            set
            {
                if (Equals(value, _permissions))
                    return;

                _permissions = value;
                OnPropertyChanged("Permissions");
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