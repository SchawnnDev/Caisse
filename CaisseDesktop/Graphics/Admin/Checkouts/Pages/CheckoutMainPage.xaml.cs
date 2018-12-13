using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    /// Interaction logic for CheckoutMainPage.xaml
    /// </summary>
    public partial class CheckoutMainPage
    {
        private CheckoutManager Manager { get; }
        private ObservableCollection<SaveableCheckoutType> Types { get; set; }
        private ObservableCollection<SaveableOwner> Owners { get; set; }

        public CheckoutMainPage(CheckoutManager manager)
        {
            InitializeComponent();
            Manager = manager;


            Task.Run(() => LoadInfos());
        }

        private void LoadInfos()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            ObservableCollection<SaveableCheckoutType> types;
            ObservableCollection<SaveableOwner> owners;

            using (var db = new CaisseServerContext())
            {
                types = new ObservableCollection<SaveableCheckoutType>(db.CheckoutTypes.OrderBy(e => e.Name).ToList());
                owners = new ObservableCollection<SaveableOwner>(db.Owners
                    .Where(t => t.Event.Id == Manager.EventManager.Evenement.Id || t.SuperAdmin)
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

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Blocage_OnClick(object sender, RoutedEventArgs e)
        {
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

        public override bool CanClose() => true;

        public override bool CanBack() => true;

        public override string CustomName => "CheckoutMainPage";
    }
}