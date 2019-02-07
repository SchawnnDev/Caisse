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
using System.Windows.Shapes;
using CaisseLibrary;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    /// Interaction logic for Parameters.xaml
    /// </summary>
    public partial class Parameters
    {
        public Parameters()
        {
            InitializeComponent();


            foreach (var e in Main.LoadEvents())
            {
                var item = new ComboBoxItem
                {
                    Content = e.Name,
                    DataContext = e
                };

                EventBox.Items.Add(item);
            }
        }

        private void ChangeCheckoutBoxItems(List<SaveableCheckout> checkouts)
        {
            CheckoutBox.Items.Clear();

            CheckoutBox.Items.Add(new ComboBoxItem { Content = "Aucune" });

            CheckoutBox.SelectedIndex = 0;

            if (checkouts.Count == 0)
            {
                CheckoutBox.IsEnabled = false;
                return;
            }

            foreach (var checkout in checkouts)
            {
                var item = new ComboBoxItem
                {
                    Content = $"[{checkout.CheckoutType.Name}] {checkout.Name}",
                    DataContext = checkout
                };

                CheckoutBox.Items.Add(item);
            }

            CheckoutBox.IsEnabled = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EventBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || !(e.AddedItems[0] is ComboBoxItem addedItem)) return;

            if (e.RemovedItems.Count != 0 && e.RemovedItems[0] is ComboBoxItem removedItem && removedItem.Content.Equals("Aucun"))
                    EventBox.Items.Remove(removedItem);

            if (!(addedItem.DataContext is SaveableEvent saveableEvent)) return;

            ChangeCheckoutBoxItems(Main.LoadCheckouts(saveableEvent.Id));

        }
    }
}