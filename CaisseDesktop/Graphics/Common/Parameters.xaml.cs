using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CaisseDesktop.Utils;
using CaisseLibrary;
using CaisseLibrary.Concrete.Session;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    ///     Interaction logic for Parameters.xaml
    /// </summary>
    public partial class Parameters
    {
        public Parameters(Login parentWindow)
        {
            InitializeComponent();

            var valueExists = false;
            Starting = true;
            ParentWindow = parentWindow;
            Owner = parentWindow; // center of screen

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
                for (var i = 1; i < EventBox.Items.Count; i++)
                {
                    var item = EventBox.Items[i];

                    if (item == null || !(item is ComboBoxItem comboBoxItem)) continue;
                    if (!(comboBoxItem.DataContext is SaveableEvent saveableEvent)) continue;
                    var eventId = int.Parse(config["event"]);
                    if (saveableEvent.Id != eventId) continue;
                    EventBox.SelectedIndex = i;
                    EventBox.Items.RemoveAt(0);
                    ChangeCheckoutBoxItems(Main.LoadCheckouts(eventId));
                }

                // load checkouts , etc

                if (CheckoutBox.Items.Count != 1)
                    for (var i = 1; i < EventBox.Items.Count; i++)
                    {
                        var item = CheckoutBox.Items[i];

                        if (item == null || !(item is ComboBoxItem comboBoxItem)) continue;
                        if (!(comboBoxItem.DataContext is SaveableCheckout saveableCheckout)) continue;
                        if (saveableCheckout.Id != int.Parse(config["checkout"])) continue;
                        CheckoutBox.SelectedIndex = i;
                        CheckoutBox.Items.RemoveAt(0);
                        valueExists = true;
                    }
            }

            Starting = false;

            if (!valueExists)
                New = true;

            Closing += OnClosing;
        }

        private bool New { get; set; }
        private bool Starting { get; }
        private Login ParentWindow { get; }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!New)
                return;

            if (Validations.WillClose(false))
                Application.Current.Shutdown();
            else
                e.Cancel = true;
        }

        private void ChangeCheckoutBoxItems(List<SaveableCheckout> checkouts)
        {
            CheckoutBox.Items.Clear();

            CheckoutBox.Items.Add(new ComboBoxItem {Content = "Aucune"});

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
                checkoutItem.Content.Equals("Aucune") ||
                !(checkoutItem.DataContext is SaveableCheckout saveableCheckout))
            {
                Validations.ShowWarning("Veuillez sélectionner une caisse");
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
            ParentWindow.UpdateLabels();
            Close();
        }

        private void EventBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Starting) return;

            if (e.AddedItems.Count == 0 || !(e.AddedItems[0] is ComboBoxItem addedItem)) return;

            if (e.RemovedItems.Count != 0 && e.RemovedItems[0] is ComboBoxItem removedItem &&
                removedItem.Content.Equals("Aucun"))
                EventBox.Items.Remove(removedItem);

            if (!(addedItem.DataContext is SaveableEvent saveableEvent)) return;

            ChangeCheckoutBoxItems(Main.LoadCheckouts(saveableEvent.Id));
        }

        private void CheckoutBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Starting) return;

            if (e.RemovedItems.Count == 0 || !(e.RemovedItems[0] is ComboBoxItem removedItem) ||
                !removedItem.Content.Equals("Aucune")) return;

            CheckoutBox.Items.Remove(removedItem);
        }
    }
}