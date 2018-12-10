using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Media;
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
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    /// Interaction logic for EvenementManager.xaml
    /// </summary>
    public partial class EvenementManager : Window
    {
        public SaveableEvent Evenement { set; get; }
        private CaisseModel CaisseModel => CheckoutsGrid.DataContext as CaisseModel;
        private bool New { get; } = true;
        private bool Saved { get; set; } = false;
        private bool Blocked { get; set; } = false;
        private bool IsBack { get; set; } = false;
        private EvenementBrowser Instance { get; }

        public EvenementManager(EvenementBrowser instance, SaveableEvent evenement)
        {
            InitializeComponent();
            Evenement = evenement;
            Instance = instance;

            if (evenement != null)
            {
                FillTextBoxes();
                New = false;
                Saved = true;
                ToggleBlocked(true);
            }
            else
            {
                //Blocage.IsChecked = false;
            }

            Closing += OnWindowClosing;

            Task.Run(() => Load());
        }

        private void ToggleBlocked(bool blocked)
        {
            Blocked = blocked;
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                CheckoutsGrid.DataContext = new CaisseModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            ObservableCollection<SaveableCheckout> checkoutsCollection;

            if (New)
            {
                checkoutsCollection = new ObservableCollection<SaveableCheckout>();
            }
            else
            {
                using (var db = new CaisseServerContext())
                {
                    checkoutsCollection = new ObservableCollection<SaveableCheckout>(db.Checkouts
                        .Where(t => t.SaveableEvent.Id == Evenement.Id).ToList());
                }
            }

            Dispatcher.Invoke(() =>
            {
                CaisseModel.Caisses = checkoutsCollection;
                Mouse.OverrideCursor = null;

                if (New) return;
                Saved = true;
                ToggleBlocked(true);
            });
        }

        private void FillTextBoxes()
        {
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableCheckout checkout)
            {
                new CheckoutManager(this, checkout).ShowDialog();
            }
            else
            {
                MessageBox.Show($"{btn} : la caisse n'est pas valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }



        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if (Saved == false && Validations.WillClose(true) == false) return;

            IsBack = true;
            Close();
            Instance.Show();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (IsBack || Saved || (Saved == false && Validations.WillClose(true))) return;

            e.Cancel = true;
        }

        private void Blocage_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Saved)
            {
                MessageBox.Show("Veuillez enregistrer avant.");
                //Blocage.IsChecked = false;
                return;
            }

            ToggleBlocked(false);
            Saved = false;
        }

        private void CreateCheckout_OnClick(object sender, RoutedEventArgs e)
        {
            new CheckoutManager(this, null).ShowDialog();
        }
    }
}