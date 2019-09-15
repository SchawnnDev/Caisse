using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CaisseDesktop.Models.Admin.Checkouts;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
	/// <summary>
	///     Interaction logic for CheckoutMainPage.xaml
	/// </summary>
	public partial class CheckoutMainPage
	{

		private CheckoutConfigModel Model => DataContext as CheckoutConfigModel;

		public CheckoutMainPage(CheckoutManager parent)
		{
			InitializeComponent();
			ParentWindow = parent;
			DataContext = new CheckoutConfigModel(parent.Checkout, parent.Checkout == null);
			Model.Dispatcher = Dispatcher;

			// Load infos
			Task.Run(() => Model.LoadInfos(parent.ParentWindow.Evenement));
			/*
            if (ParentWindow.Checkout != null)
            {
                FillTextBoxes();
                New = false;
                Saved = true;
                ToggleBlocked(true);
            }
            else
            {
                Blocage.IsChecked = false;
            }

            Task.Run(() => LoadInfos()); */
		}

		private CheckoutManager ParentWindow { get; }
		private bool Saved { get; set; }
		private bool New { get; } = true;
		private bool Blocked { get; set; }

		public override string CustomName => "CheckoutMainPage";

		private void ToggleBlocked(bool blocked)
		{
			CheckoutName.IsEnabled = !blocked;
			CheckoutType.IsEnabled = !blocked;
			CheckoutOwner.IsEnabled = !blocked;
			CheckoutInfos.IsEnabled = !blocked;
			CheckoutSave.IsEnabled = !blocked;
			Blocage.IsChecked = blocked;
			Blocked = blocked;
		}

		private void FillTextBoxes()
		{
			CheckoutName.Text = ParentWindow.Checkout.Name;
			CheckoutInfos.Text = ParentWindow.Checkout.Details;
		}

		private void Save_OnClick(object sender, RoutedEventArgs e)
		{
			if (Check(CheckoutName) || string.IsNullOrWhiteSpace(CheckoutType.Text) && Check(CheckoutType) ||
				Check(CheckoutOwner) || Check(CheckoutInfos))
				return;

			if (ParentWindow.Checkout == null)
				ParentWindow.Checkout = new SaveableCheckout();

			ParentWindow.Checkout.Name = CheckoutName.Text;
			ParentWindow.Checkout.Details = CheckoutInfos.Text;
			ParentWindow.Checkout.Owner = (SaveableOwner)CheckoutOwner.SelectedItem;

			//  if (Types.Any(t => t.Name.Equals(CheckoutType.Text)))
			// {
			ParentWindow.Checkout.CheckoutType = (SaveableCheckoutType)CheckoutType.SelectedItem;
			//}

			Task.Run(() => Save());
		}

		private void Save()
		{
			Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

			using (var db = new CaisseServerContext())
			{
				db.CheckoutTypes.Attach(ParentWindow.Checkout.CheckoutType);
				//db.Events.Attach(ParentWindow.Checkout.CheckoutType.Event);
				db.Owners.Attach(ParentWindow.Checkout.Owner);

				if (db.CheckoutTypes.Any(t => t.Event.Id == ParentWindow.Checkout.CheckoutType.Id))
					db.CheckoutTypes.Attach(ParentWindow.Checkout.CheckoutType);
				else
					db.CheckoutTypes.Add(ParentWindow.Checkout.CheckoutType);

				db.Entry(ParentWindow.Checkout).State = New ? EntityState.Added : EntityState.Modified;
				db.SaveChanges();
			}

			Dispatcher.Invoke(() =>
			{
				Mouse.OverrideCursor = null;
				MessageBox.Show(New ? "La caisse a bien été crée !" : "La caisse a bien été enregistré !");


				if (ParentWindow.ParentWindow.CurrentPage.Equals("EventCheckoutPage"))
				{
					if (New)
						ParentWindow.ParentWindow.CurrentPage.Add(ParentWindow.Checkout);
					else
						ParentWindow.ParentWindow.CurrentPage.Update();
				}

				ToggleBlocked(true);
				Saved = true;
			});
		}

		private void Blocage_OnClick(object sender, RoutedEventArgs e)
		{
			if (!Saved)
			{
				MessageBox.Show("Veuillez enregistrer avant.");
				Blocage.IsChecked = false;
				return;
			}

			ToggleBlocked(false);
			Saved = false;
		}

		public override bool CanClose()
		{
			return Saved || !Saved && Validations.WillClose(true);
		}

		public override bool CanBack()
		{
			return Saved || Validations.WillClose(true);
		}

		public override void Add<T>(T t)
		{
		}

		public override void Update()
		{
		}
	}
}