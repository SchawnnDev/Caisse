using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    /// Interaction logic for CheckoutMainPage.xaml
    /// </summary>
    public partial class CheckoutMainPage
    {
        private CheckoutManager ParentWindow { get; }
        private ObservableCollection<SaveableCheckoutType> Types { get; set; }
        private ObservableCollection<SaveableOwner> Owners { get; set; }
        private bool Saved { get; set; }
        private bool New { get; set; } = true;
        private bool Blocked { get; set; }

        public CheckoutMainPage(CheckoutManager parent)
        {
            InitializeComponent();
            ParentWindow = parent;

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

            Task.Run(() => LoadInfos());
        }

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
            if (Check(CheckoutName) || Check(CheckoutInfos) ||
                Check(CheckoutOwner) || Check(CheckoutType))
                return;

            if (ParentWindow.Checkout == null)
                ParentWindow.Checkout = new SaveableCheckout();

            ParentWindow.Checkout.Name = CheckoutName.Text;
            ParentWindow.Checkout.Details = CheckoutInfos.Text;
            ParentWindow.Checkout.CheckoutType = (SaveableCheckoutType)CheckoutType.SelectedItem;
            ParentWindow.Checkout.Owner = (SaveableOwner)CheckoutOwner.SelectedItem;

            Task.Run(() => Save());
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                db.Checkouts.AddOrUpdate(ParentWindow.Checkout);
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "La caisse a bien été crée !" : "La caisse a bien été enregistré !");
                if (New) ParentWindow.EventManager.Add(ParentWindow.Evenement);
                else ParentWindow.ParentWindow.Update();
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

        public override bool CanClose() => Saved || !Saved && Validations.WillClose(true);

        public override bool CanBack() => Saved || Validations.WillClose(true);

        private void LoadInfos()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            ObservableCollection<SaveableCheckoutType> types;
            ObservableCollection<SaveableOwner> owners;

            using (var db = new CaisseServerContext())
            {
                types = new ObservableCollection<SaveableCheckoutType>(db.CheckoutTypes.OrderBy(e => e.Event.Id == Manager.EventManager.Evenement.Id).ToList());
                owners = new ObservableCollection<SaveableOwner>(db.Owners
                    .Where(t => t.Event.Id == Manager.EventManager.Evenement.Id)
                    .OrderBy(e => e.LastLogin).ToList()); 
            }

            Dispatcher.Invoke(() =>
            {
                Types = types;
                Owners = owners;
                CheckoutType.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = Types});
                Owner.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = Owners});
                Mouse.OverrideCursor = null;
            });
        }

        private void CheckoutType_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox) sender;
            if (comboBox.SelectedItem != null)
                return;

            using (var db = new CaisseServerContext())
            {
                var newItem = new SaveableCheckoutType
                {
                    Name = comboBox.Text
                };
                Types.Add(newItem);
                comboBox.SelectedItem = newItem;
                db.CheckoutTypes.Add(newItem);
            }
        }

        public override string CustomName => "CheckoutMainPage";
    }
}