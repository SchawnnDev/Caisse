using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Common;
using CaisseDesktop.Graphics.Utils;

namespace CaisseDesktop.Models.Checkouts.Common
{
    public class LoginModel : INotifyPropertyChanged
    {
        private readonly int PASSWORD_LENGTH = 7;
        private readonly DispatcherTimer _timer;
        private ICommand _connectCommand;
        public ICommand ConnectCommand => _connectCommand ?? (_connectCommand = new CommandHandler(Connect, true));

        private ICommand _pinPadCommand;
        public ICommand PinPadCommand => _pinPadCommand ?? (_pinPadCommand = new CommandHandler(PinPad, true));

        private ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new CommandHandler(Cancel, true));


        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new CommandHandler(Delete, true));

        public LoginModel()
        {
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => OnPropertyChanged("CurrentTime");
            _timer.Start();

            LoginPassword = "";
        }


        private string _loginPassword;

        public string LoginPassword
        {
            get => _loginPassword;
            set
            {
                var val = value;
                if (val.Length > PASSWORD_LENGTH) return;
                _loginPassword = value;
                OnPropertyChanged();
            }
        }

        public DateTime CurrentTime => DateTime.Now;


        private string _checkoutName;

        public string CheckoutName
        {
            get => "Caisse: " + _checkoutName;
            set
            {
                _checkoutName = value;
                OnPropertyChanged();
            }
        }

        private void Delete(object param)
        {
        }

        private void Cancel(object param)
        {
        }

        public void PinPad(object param)
        {
            if (LoginPassword.Length == PASSWORD_LENGTH) return;
            LoginPassword += ((int) param).ToString();

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