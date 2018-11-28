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
using CaisseDesktop.Models;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    /// Interaction logic for EvenementManager.xaml
    /// </summary>
    public partial class EvenementManager : Window
    {
        public SaveableEvent Evenement { get; }
        private JourModel Model => DataContext as JourModel;


        public EvenementManager(SaveableEvent evenement)
        {
            InitializeComponent();
            Evenement = evenement;

            if (evenement != null)
                FillTextBoxes();

            Task.Run(() => Load());
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = new JourModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            ObservableCollection<SaveableDay> collection;

            using (var db = new CaisseServerContext())
                collection = new ObservableCollection<SaveableDay>(db.Days.Where(t => t.Event.Id == Evenement.Id)
                    .OrderBy(e => e.End).ToList());

            Dispatcher.Invoke(() =>
            {
                Model.Jours = collection;
                Mouse.OverrideCursor = null;
            });
        }

        private void FillTextBoxes()
        {
            EventName.Text = Evenement.Name;
            EventStart.DefaultValue = Evenement.Start;
            EventEnd.DefaultValue = Evenement.End;
            EventDescription.Text = Evenement.Description;
            EventAddresse.Text = Evenement.Addresse;
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}