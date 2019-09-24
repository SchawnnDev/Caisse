using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Graphics.Admin.CheckoutTypes;
using CaisseDesktop.Models;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    ///     Interaction logic for EventCheckoutTypePage.xaml
    /// </summary>
    public partial class EventCheckoutTypePage
    {
        private EvenementManager Manager { get; }
        private bool New { get; set; }
        private CheckoutTypeModel Model => DataContext as CheckoutTypeModel;

        public EventCheckoutTypePage(EvenementManager manager)
        {
            InitializeComponent();
            Manager = manager;
            New = manager.Evenement == null;
            Task.Run(() => Load());
        }

        public override string CustomName => "EventCheckoutTypePage";

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableCheckoutType type)
                new CheckoutTypeManager(Manager, type).ShowDialog();
            else
                MessageBox.Show($"{btn} : le type de caisse n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = new CheckoutTypeModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            var checkoutTypesCollection = new ObservableCollection<SaveableCheckoutType>();

            if (!New)
                using (var db = new CaisseServerContext())
                {
                    checkoutTypesCollection = new ObservableCollection<SaveableCheckoutType>(db.CheckoutTypes
                        .Where(t => t.Event.Id == Manager.Evenement.Id).Include(t=>t.Event)
                        .ToList());
                }

            Dispatcher.Invoke(() =>
            {
                Model.CheckoutTypes = checkoutTypesCollection;
                Mouse.OverrideCursor = null;
            });
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }

        public override void Update()
        {
            CheckoutsGrid.Items.Refresh();
        }

        public override void Add<T>(T item)
        {
            Model.CheckoutTypes.Add(item as SaveableCheckoutType);
        }

        public override bool CanClose()
        {
            return true;
        }

        public override bool CanBack()
        {
            return true;
        }
    }
}