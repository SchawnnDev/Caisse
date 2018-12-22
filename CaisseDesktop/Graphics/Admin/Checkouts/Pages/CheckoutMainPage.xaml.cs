using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Events.Pages;
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
            if (Check(CheckoutName) || string.IsNullOrWhiteSpace(CheckoutType.Text) && Check(CheckoutType) ||
                Check(CheckoutOwner) || Check(CheckoutInfos))
                return;

            if (ParentWindow.Checkout == null)
            {
                ParentWindow.Checkout = new SaveableCheckout
                {
                    SaveableEvent = ParentWindow.EventManager.Evenement
                };
            }

            ParentWindow.Checkout.Name = CheckoutName.Text;
            ParentWindow.Checkout.Details = CheckoutInfos.Text;
            ParentWindow.Checkout.CheckoutType = (SaveableCheckoutType) CheckoutType.SelectedItem;
            ParentWindow.Checkout.Owner = (SaveableOwner) CheckoutOwner.SelectedItem;

            Task.Run(() => Save());
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            var type = ParentWindow.Checkout.CheckoutType;

            using (var db = new CaisseServerContext())
            {
                db.Events.Attach(ParentWindow.Checkout.SaveableEvent);
                db.Owners.Attach(ParentWindow.Checkout.Owner);

                if (Types.Any(t => t.Name.Equals(CheckoutType.Text)))
                {
                    db.CheckoutTypes.Attach(type);
                }
                else
                {
                    type = new SaveableCheckoutType
                    {
                        Event = ParentWindow.Checkout.SaveableEvent,
                        Name = CheckoutType.Text
                    };

                    db.CheckoutTypes.Add(type);
                }


                db.Entry(ParentWindow.Checkout).State = New ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Types.Add(type);
                ParentWindow.Checkout.CheckoutType = type;
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "La caisse a bien été crée !" : "La caisse a bien été enregistré !");


                if (ParentWindow.EventManager.CurrentPage.Equals("EventCheckoutPage"))
                {
                    if (New)
                        ParentWindow.EventManager.CurrentPage.Add(ParentWindow.Checkout);
                    else
                        ParentWindow.EventManager.CurrentPage.Update();
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

        public override bool CanClose() => Saved || !Saved && Validations.WillClose(true);

        public override bool CanBack() => Saved || Validations.WillClose(true);

        private void LoadInfos()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            ObservableCollection<SaveableCheckoutType> types;
            ObservableCollection<SaveableOwner> owners;

            using (var db = new CaisseServerContext())
            {
                types = new ObservableCollection<SaveableCheckoutType>(db.CheckoutTypes
                    .OrderBy(e => e.Event.Id == ParentWindow.EventManager.Evenement.Id).ToList());
                owners = new ObservableCollection<SaveableOwner>(db.Owners
                    .Where(t => t.Event.Id == ParentWindow.EventManager.Evenement.Id)
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
                    CheckoutType.SelectedItem = Types.FindIndex(t => t.Id == ParentWindow.Checkout.CheckoutType.Id);
                    CheckoutOwner.SelectedItem = Owners.FindIndex(t => t.Id == ParentWindow.Checkout.Owner.Id);
                }

                Mouse.OverrideCursor = null;
            });
        }

        /*

        private void CheckoutType_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox) sender;
            if (comboBox.SelectedItem != null)
                return;

            using (var db = new CaisseServerContext())
            {
                db.Events.Attach(ParentWindow.Checkout.SaveableEvent);
                var newItem = new SaveableCheckoutType
                {
                    Name = comboBox.Text
                };
                Types.Add(newItem);
                comboBox.SelectedItem = newItem;
                db.CheckoutTypes.Add(newItem);
            }
        } */

        public override void Add<T>(T t)
        {
        }

        public override void Update()
        {
        }

        public override string CustomName => "CheckoutMainPage";
    }
}