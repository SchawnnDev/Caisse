using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Common;
using CaisseDesktop.Graphics.Common.Checkouts;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Utils;
using CaisseLibrary;
using CaisseLibrary.Data;

namespace CaisseDesktop.Models.Checkouts.Common
{
    public class LoginModel : INotifyPropertyChanged
    {
        private readonly int PASSWORD_LENGTH = 7;
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
	        var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
	        timer.Tick += (s, e) => OnPropertyChanged($"CurrentTime");
            timer.Start();

	        CheckoutName = Main.ActualCheckout?.Name;
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

	        if (box.Password.Length != 7)
	        {
		        SystemSounds.Beep.Play();
		        MessageBox.Show("L'identifiant n'est pas valide.", "Erreur", MessageBoxButton.OK,
			        MessageBoxImage.Error);
		        return;
	        }

	        var cashier = Main.Login(box.Password);

	        if (cashier == null)
	        {
		        MessageBox.Show("L'identifiant n'est pas valide.", "Erreur", MessageBoxButton.OK,
			        MessageBoxImage.Error);
		        return;
	        }

	        Main.ActualCashier = cashier;
	       // new Checkout(Main.ActualCheckout).Show();

	        switch (Main.ActualCheckout.CheckoutType.Type)
	        {
				case 0: // tickets
					new TicketCheckout(Main.ActualCheckout).Show();
					break;
				case 1: // articles
					new ArticleCheckout(Main.ActualCheckout).Show();
					break;
				case 2: // consigns
					new ConsignCheckout(Main.ActualCheckout).Show();
					break;
				default:
					break;
	        }

	        //TODO: Si ce n'est pas encore l'heure du caissier, le prevenir et demander si il est sûr de vouloir continuer
	        //MessageBox.Show("Bien connecté.", "Yeah", MessageBoxButton.OK, MessageBoxImage.Hand);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

	}
}