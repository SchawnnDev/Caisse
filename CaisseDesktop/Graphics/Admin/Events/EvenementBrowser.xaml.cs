using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Models;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    ///     Interaction logic for EvenementBrowser.xaml
    /// </summary>
    public partial class EvenementBrowser
    {
        public EvenementBrowser()
        {
            InitializeComponent();

            Task.Run(() => Load());
        }

        private EvenementModel Model => DataContext as EvenementModel;
        private bool IsLoading { get; set; }

        public void Add(SaveableEvent e)
        {
            Model.Evenements.Add(e);
        }

        public void Update()
        {
            EventsGrid.Items.Refresh();
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                IsLoading = true;
                DataContext = new EvenementModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            ObservableCollection<SaveableEvent> collection;

            using (var db = new CaisseServerContext())
            {
                collection = new ObservableCollection<SaveableEvent>(db.Events.OrderBy(e => e.End).ToList());
            }

            Dispatcher.Invoke(() =>
            {
                Model.Evenements = collection;
                Mouse.OverrideCursor = null;
                IsLoading = false;
            });
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableEvent evenement)
            {
                new EvenementManager(evenement).Show();
                Close();
            }
            else
            {
                MessageBox.Show($"{btn} : l'événement n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableEvent evenement)
            {
                var result = MessageBox.Show("Es tu sûr de vouloir supprimer cet événement ?", "Supprimer un événement",
                    MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result != MessageBoxResult.Yes) return;

                using (var db = new CaisseServerContext())
                {
                    db.Events.Attach(evenement);
                    db.Events.Remove(evenement);
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show($"{btn} : l'événement n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CreateEvent_OnClick(object sender, RoutedEventArgs e)
        {
            new EvenementManager(null).Show();
            Close();
        }

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			new SelectionWindow().Show();
			Close();
		}
	}
}