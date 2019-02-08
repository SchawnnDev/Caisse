using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using CaisseDesktop.Utils;
using CaisseLibrary;
using CaisseLibrary.Concrete.Session;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    /// Interaction logic for Parameters.xaml
    /// </summary>
    public partial class Parameters
    {

        private bool New { get; set; }

        public Parameters()
        {
            InitializeComponent();

            var config = ConfigFile.GetConfig();

            New = !config.ContainsKey("event") || !config.ContainsKey("checkout");

            foreach (var e in Main.LoadEvents())
            {
                var item = new ComboBoxItem
                {
                    Content = e.Name,
                    DataContext = e
                };

                EventBox.Items.Add(item);
            }

            if (!New && EventBox.Items.Count != 1) // if the list is empty, the event doesnt exists anymore
            {
                EventBox.Items.RemoveAt(0);
                for (var i = 0; i < EventBox.Items.Count; i++)
                {
                    var item = EventBox.Items[i];

                    if (item == null || !(item is ComboBoxItem comboBoxItem)) break;
                    if (!(comboBoxItem.DataContext is SaveableEvent saveableEvent)) break;
                    if (saveableEvent.Id != int.Parse(config["event"])) break;
                    EventBox.SelectedIndex = i;
                }

                // load checkouts , etc
                

            }

            Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {

            if (New)
            {

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

            if (EventBox.SelectedItem == null || !(EventBox.SelectedItem is ComboBoxItem eventItem) ||
                eventItem.Content.Equals("Aucun") || !(eventItem.DataContext is SaveableEvent saveableEvent))
            {
                Validations.ShowWarning("Veuillez selectionner un évenement");
                return;
            }

            if (CheckoutBox.SelectedItem == null || !(CheckoutBox.SelectedItem is ComboBoxItem checkoutItem) ||
                checkoutItem.Content.Equals("Aucune") || !(checkoutItem.DataContext is SaveableCheckout saveableCheckout))
            {
                Validations.ShowWarning("Veuillez selectionner une caisse");
                return;
            }

            ConfigFile.SetValues(new Dictionary<string, string>
                {
                    {"event", saveableEvent.Id.ToString()}, {"checkout", saveableCheckout.Id.ToString()}
                });

            Main.ActualEvent = saveableEvent;
            CheckoutSession.ActualCheckout = saveableCheckout;
            // validate
            New = false;
            Close();


        }

        private void EventBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || !(e.AddedItems[0] is ComboBoxItem addedItem)) return;

            if (e.RemovedItems.Count != 0 && e.RemovedItems[0] is ComboBoxItem removedItem && removedItem.Content.Equals("Aucun"))
                EventBox.Items.Remove(removedItem);

            if (!(addedItem.DataContext is SaveableEvent saveableEvent)) return;

            ChangeCheckoutBoxItems(Main.LoadCheckouts(saveableEvent.Id));

        }

        private void CheckoutBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 0 || !(e.RemovedItems[0] is ComboBoxItem removedItem) ||
                !removedItem.Content.Equals("Aucune")) return;

            CheckoutBox.Items.Remove(removedItem);

        }
    }
}