using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Owners;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Admin;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    ///     Interaction logic for EventOwnerPage.xaml
    /// </summary>
    public partial class EventOwnerPage
    {
        public EventOwnerPage(EventManagerModel parentModel)
        {
            InitializeComponent();
            ParentModel = parentModel;

            New = parentModel.SaveableEvent == null;

            Task.Run(Load);
        }

        private ResponsableModel ResponsableModel => OwnersGrid.DataContext as ResponsableModel;
        private bool New { get; }
        private EventManagerModel ParentModel { get; }

        public override string CustomName => "EventOwnerPage";

        public override void Add<T>(T t)
        {
            ResponsableModel.Responables.Add(t as SaveableOwner);
        }

        public override void Update()
        {
            OwnersGrid.Items.Refresh();
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
                using (var db = new CaisseServerContext())
                {
                    checkoutsCollection = new ObservableCollection<SaveableOwner>(db.Owners
                        .Where(t => t.Event.Id == ParentModel.SaveableEvent.Id).OrderByDescending(t => t.LastLogin)
                        .ToList());
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
	    //        new OwnerManager(ParentWindow, owner).ShowDialog();
            }
            else
                MessageBox.Show($"{btn} : le résponsable n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableOwner owner)
            {
                var result = MessageBox.Show("Es tu sûr de vouloir supprimer ce résponsable ?",
                    "Supprimer un résponsable",
                    MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result != MessageBoxResult.Yes) return;

                using (var db = new CaisseServerContext())
                {
                    db.Owners.Attach(owner);
                    db.Owners.Remove(owner);
                    db.SaveChanges();
                }

                ResponsableModel.Responables.Remove(owner);
            }
            else
            {
                MessageBox.Show($"{btn} : le résponsable n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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