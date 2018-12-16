using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Models;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    /// Interaction logic for EventOwnerPage.xaml
    /// </summary>
    public partial class EventOwnerPage
    {
        private ResponsableModel ResponsableModel => OwnersGrid.DataContext as ResponsableModel;
        private bool New { get; }
        private EvenementManager ParentWindow { get; }

        public EventOwnerPage(EvenementManager parentWindow)
        {
            InitializeComponent();
            ParentWindow = parentWindow;

            New = parentWindow.Evenement == null;

            Task.Run(() => Load());
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                OwnersGrid.DataContext = new ResponsableModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            var checkoutsCollection = new ObservableCollection<SaveableOwner>();

            if (!New)
            {
                using (var db = new CaisseServerContext())
                {
                    checkoutsCollection = new ObservableCollection<SaveableOwner>(db.Owners
                        .Where(t => t.Event.Id == ParentWindow.Evenement.Id).OrderByDescending(t => t.LastLogin)
                        .ToList());
                }
            }

            Dispatcher.Invoke(() =>
            {
                ResponsableModel.Responables = checkoutsCollection;
                Mouse.OverrideCursor = null;
            });
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableOwner owner)
            {
                //new CheckoutManager(ParentWindow, checkout).ShowDialog();
            }
            else
            {
                MessageBox.Show($"{btn} : le résponsable n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }

        public override bool CanClose() => true;

        public override bool CanBack() => true;

        public override string CustomName => "EventOwnerPage";
    }
}