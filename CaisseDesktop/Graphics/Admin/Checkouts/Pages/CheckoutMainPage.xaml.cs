using System.Collections.ObjectModel;
using System.Data.Entity;
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
    ///     Interaction logic for CheckoutMainPage.xaml
    /// </summary>
    public partial class CheckoutMainPage
    {
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

        private CheckoutManager ParentWindow { get; }
        private ObservableCollection<SaveableCheckoutType> Types { get; set; }
        private ObservableCollection<SaveableOwner> Owners { get; set; }
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
            ParentWindow.Checkout.Owner = (SaveableOwner) CheckoutOwner.SelectedItem;

            if (Types.Any(t => t.Name.Equals(CheckoutType.Text)))
            {
                ParentWindow.Checkout.CheckoutType = (SaveableCheckoutType) CheckoutType.SelectedItem;
            }

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

        private void LoadInfos()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            ObservableCollection<SaveableCheckoutType> types;
            ObservableCollection<SaveableOwner> owners;

            using (var db = new CaisseServerContext())
            {
                types = new ObservableCollection<SaveableCheckoutType>(db.CheckoutTypes
                    .OrderBy(e => e.Event.Id == ParentWindow.ParentWindow.Evenement.Id).ToList());
                owners = new ObservableCollection<SaveableOwner>(db.Owners
                    .Where(t => t.Event.Id == ParentWindow.ParentWindow.Evenement.Id)
                    .OrderBy(e => e.LastLogin).ToList());
            }

            Dispatcher.Invoke(() =>
            {
                Types = types;
                Owners = owners;
                CheckoutType.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = Types});
                CheckoutOwner.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = Owners});

                if (!New)
                {
                    CheckoutType.SelectedIndex = Types.FindIndex(t => t.Id == ParentWindow.Checkout.CheckoutType.Id);
                    CheckoutOwner.SelectedIndex = Owners.FindIndex(t => t.Id == ParentWindow.Checkout.Owner.Id);
                }

                Mouse.OverrideCursor = null;
            });
        }

        public override void Add<T>(T t)
        {
        }

        public override void Update()
        {
        }
    }
}