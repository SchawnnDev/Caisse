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
        private JourModel Model => DaysGrid.DataContext as JourModel;
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
                Blocage.IsChecked = false;
            }

            Closing += OnWindowClosing;

            Task.Run(() => Load());
        }

        private void ToggleBlocked(bool blocked)
        {
            EventName.IsEnabled = !blocked;
            EventAddresse.IsEnabled = !blocked;
            EventDescription.IsEnabled = !blocked;
            EventStart.IsEnabled = !blocked;
            EventEnd.IsEnabled = !blocked;
            EventSave.IsEnabled = !blocked;
            Blocage.IsChecked = blocked;
            Blocked = blocked;
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                DaysGrid.DataContext = new JourModel();
                CheckoutsGrid.DataContext = new CaisseModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            ObservableCollection<SaveableDay> daysCollection;
            ObservableCollection<SaveableCheckout> checkoutsCollection;

            if (New)
            {
                daysCollection = new ObservableCollection<SaveableDay>();
                checkoutsCollection = new ObservableCollection<SaveableCheckout>();
            }
            else
            {
                using (var db = new CaisseServerContext())
                {
                    daysCollection = new ObservableCollection<SaveableDay>(db.Days
                        .Where(t => t.Event.Id == Evenement.Id)
                        .OrderBy(e => e.End).ToList());
                    checkoutsCollection = new ObservableCollection<SaveableCheckout>(db.Checkouts
                        .Where(t => t.SaveableEvent.Id == Evenement.Id).ToList());
                }
            }

            Dispatcher.Invoke(() =>
            {
                Model.Jours = daysCollection;
                CaisseModel.Caisses = checkoutsCollection;
                Mouse.OverrideCursor = null;

                if (New) return;
                Saved = true;
                ToggleBlocked(true);
            });
        }

        private void FillTextBoxes()
        {
            EventName.Text = Evenement.Name;
            EventStart.Value = Evenement.Start;
            EventEnd.Value = Evenement.End;
            EventDescription.Text = Evenement.Description;
            EventAddresse.Text = Evenement.Addresse;
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (Check(EventName) || Check(EventStart) ||
                Check(EventEnd) || Check(EventAddresse) || Check(EventDescription))
                return;

            if (Evenement == null)
                Evenement = new SaveableEvent();

            Evenement.Name = EventName.Text;
            Evenement.Description = EventDescription.Text;
            Evenement.Addresse = EventAddresse.Text;
            Evenement.Start = EventStart.Value.GetValueOrDefault();
            Evenement.End = EventEnd.Value.GetValueOrDefault();

            Task.Run(() => Save());
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                db.Events.AddOrUpdate(Evenement);
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "L'événement à bien été crée !" : "L'événement à bien été enregistré !");
                if (New) Instance.Add(Evenement);
                else Instance.Update();
                ToggleBlocked(true);
                Saved = true;
            });
        }

        private bool Check(DateTimePicker picker)
        {
            var date = picker.Value;
            if (date != null) return false;
            picker.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
        }

        private bool Check(TextBox box)
        {
            var str = box.Text;
            if (!string.IsNullOrWhiteSpace(str)) return false;
            box.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
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
                Blocage.IsChecked = false;
                return;
            }

            ToggleBlocked(false);
            Saved = false;
        }
    }
}