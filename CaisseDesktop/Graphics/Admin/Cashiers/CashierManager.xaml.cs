using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Media;
using CaisseDesktop.Admin;
using CaisseDesktop.Graphics.Admin.TimeSlots;
using CaisseDesktop.Utils;
using CaisseLibrary.Concrete.Owners;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Cashiers
{
	/// <summary>
	///     Interaction logic for OwnerManager.xaml
	/// </summary>
	public partial class CashierManager
	{
		public CashierManager(TimeSlotManager parentWindow, SaveableCashier cashier)
		{
			InitializeComponent();
			Owner = parentWindow;
			ParentWindow = parentWindow;
			Cashier = cashier;
			Closing += OnWindowClosing;
			Saved = cashier.Id != 0; // not saved if new
			FillTextBoxes();
			CashierDelete.IsEnabled = true;
		}

		public TimeSlotManager ParentWindow { get; set; }
		public SaveableCashier Cashier { get; set; }
		private bool Saved { get; set; }

		public void OnWindowClosing(object sender, CancelEventArgs e)
		{
			if (Saved || !Saved && Validations.WillClose(true)) return;
			e.Cancel = true;
		}

		private void FillTextBoxes()
		{
			CashierFirstName.Text = Cashier.FirstName;
			CashierName.Text = Cashier.Name;
			CashierLastActivity.Text =
				$"{Cashier.LastActivity.ToLongDateString()} {Cashier.LastActivity.ToShortTimeString()}";
			CashierWasHere.IsChecked = Cashier.WasHere;
			FillLogin();
		}

		private void FillLogin()
		{
			CashierLogin.Text = SessionAdmin.HasPermission("cashiers.login.show")
				? Cashier.Login
				: new string('•', Cashier.Login.Length);
		}

		private void Save_OnClick(object sender, RoutedEventArgs e)
		{
			if (CustomPage.Check(CashierFirstName) || CustomPage.Check(CashierName)) return;

			if (string.IsNullOrWhiteSpace(Cashier.Login))
			{
				GenLogin.BorderBrush = Brushes.Red;
				SystemSounds.Beep.Play();
				return;
			}

			Cashier.FirstName = CashierFirstName.Text;
			Cashier.Name = CashierName.Text;

			Saved = true;

			// set cashier or substitute
			ParentWindow.SetCashier(Cashier);

			// direct close of dialog
			Close();
		}

		private void GenLogin_OnClick(object sender, RoutedEventArgs e)
		{
			if (SessionAdmin.HasNotPermission("owners.login.gen"))
				return;

			// possible characters : 123456789ABCXYZ/*- and length=7

			using (var db = new CaisseServerContext())
			{
				var logins = db.Cashiers.Select(t => t.Login).ToList();

				Cashier.Login = new CashierPassword().GenerateNoDuplicate(7, logins);

				FillLogin();
			}
		}

		private void Delete_OnClick(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show("Es tu sûr de vouloir supprimer ce caissier ?", "Supprimer un caissier",
				MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

			if (result != MessageBoxResult.Yes) return;

			// remove cashier

			ParentWindow.RemoveCashier(Cashier);

			Saved = true;

			// direct close of dialog
			Close();
		}

		private void CashierWasHere_OnClick(object sender, RoutedEventArgs e)
		{
			Cashier.WasHere = !Cashier.WasHere;
		}
	}
}