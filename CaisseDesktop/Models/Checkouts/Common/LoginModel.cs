using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CaisseDesktop.Graphics.Utils;

namespace CaisseDesktop.Models.Checkouts.Common
{
    public class LoginModel : INotifyPropertyChanged
    {
        private ICommand _connectCommand;
        public ICommand ConnectCommand => _connectCommand ?? (_connectCommand = new CommandHandler(Connect, true));

        private ICommand _pinPadCommand;
        public ICommand PinPadCommand => _pinPadCommand ?? (_pinPadCommand = new CommandHandler(PinPad, true));

        private ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new CommandHandler(Cancel, true));


        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new CommandHandler(Delete, true));

        private string _loginPassword;

        public string LoginPassword
        {
            get => _loginPassword;
            set
            {
                _loginPassword = value;
                OnPropertyChanged();
            }
        }

        private void Delete(object param)
        {
        }

        private void Cancel(object param)
        {
        }


        private void PinPad(object param)
        {
        }

        private void Connect(object param)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}