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
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Models;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    /// Interaction logic for EventCheckoutPage.xaml
    /// </summary>
    public partial class EventCheckoutPage
    {
        private CaisseModel CaisseModel => CheckoutsGrid.DataContext as CaisseModel;
        private bool New { get; }
        private EvenementManager ParentWindow { get; }

        public EventCheckoutPage(EvenementManager parentWindow)
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
                CheckoutsGrid.DataContext = new CaisseModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            var checkoutsCollection = new ObservableCollection<SaveableCheckout>();

            if (!New)
            {
                using (var db = new CaisseServerContext())
                {
                    checkoutsCollection = new ObservableCollection<SaveableCheckout>(db.Checkouts
                        .Where(t => t.SaveableEvent.Id == ParentWindow.Evenement.Id).ToList());
                }
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
            {
                new CheckoutManager(ParentWindow, checkout).ShowDialog();
            }
            else
            {
                MessageBox.Show($"{btn} : la caisse n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }

        public override bool CanClose() => true;

        public override bool CanBack() => true;

        public override string CustomName => "EventCheckoutPage";
    }
}