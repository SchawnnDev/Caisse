using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
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
	        _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
	        _timer.Tick += (s, e) => OnPropertyChanged($"CurrentTime");
            _timer.Start();
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
	        if (!(param is PasswordBox box)) return;

	        var length = box.Password.Length;

	        box.Password = length > 1 ? box.Password.Remove(length - 1) : "";

        }

        private void Cancel(object param)
        {
	        if (!(param is PasswordBox box)) return;

	        box.Password = "";
        }

        public void PinPad(object param)
        {
	        if (!(param is object[] array)) return;
	        var input = array[0] is int i ? i : 0;
	        if (!(array[1] is PasswordBox box)) return;
			if (box.Password.Length >= PASSWORD_LENGTH) return;
	        box.Password += input.ToString();
        }

        private void Connect(object param)
        {
	        if (!(param is PasswordBox box)) return;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

	}
}