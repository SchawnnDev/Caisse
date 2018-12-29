using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Models;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    ///     Interaction logic for EventCheckoutPage.xaml
    /// </summary>
    public partial class EventCheckoutPage
    {
        public EventCheckoutPage(EvenementManager parentWindow)
        {
            InitializeComponent();
            ParentWindow = parentWindow;

            New = parentWindow.Evenement == null;

            Task.Run(() => Load());
        }

        private CaisseModel CaisseModel => CheckoutsGrid.DataContext as CaisseModel;
        private bool New { get; }
        private EvenementManager ParentWindow { get; }

        public override string CustomName => "EventCheckoutPage";

        public override void Add<T>(T t)
        {
            CaisseModel.Caisses.Add(t as SaveableCheckout);
        }

        public override void Update()
        {
            CheckoutsGrid.Items.Refresh();
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                CheckoutsGrid.DataContext = new CaisseModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            var checkoutsCollection = new ObservableCollection<SaveableCheckout>();

            if (!New)
                using (var db = new CaisseServerContext())
                {
                    checkoutsCollection = new ObservableCollection<SaveableCheckout>(db.Checkouts
                        .Where(t => t.SaveableEvent.Id == ParentWindow.Evenement.Id).Include(t => t.CheckoutType)
                        .Include(t => t.Owner).ToList());
                }

            Dispatcher.Invoke(() =>
            {
                CaisseModel.Caisses = checkoutsCollection;
                Mouse.OverrideCursor = null;
            });
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableCheckout checkout)
                new CheckoutManager(ParentWindow, checkout).ShowDialog();
            else
                MessageBox.Show($"{btn} : la caisse n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
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