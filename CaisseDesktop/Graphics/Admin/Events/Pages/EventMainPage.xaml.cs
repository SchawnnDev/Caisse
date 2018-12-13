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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    /// Interaction logic for MainEventManager.xaml
    /// </summary>
    public partial class EventMainPage
    {
        private bool New { get; } = true;
        private bool Saved { get; set; } = false;
        private bool Blocked { get; set; }
        private EvenementManager ParentWindow { get; }

        public EventMainPage(EvenementManager parentWindow)
        {
            InitializeComponent();
            ParentWindow = parentWindow;

            if (parentWindow.Evenement != null)
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

        private void FillTextBoxes()
        {
            EventName.Text = ParentWindow.Evenement.Name;
            EventStart.Value = ParentWindow.Evenement.Start;
            EventEnd.Value = ParentWindow.Evenement.End;
            EventDescription.Text = ParentWindow.Evenement.Description;
            EventAddresse.Text = ParentWindow.Evenement.Addresse;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (Check(EventName) || Check(EventStart) ||
                Check(EventEnd) || Check(EventAddresse) || Check(EventDescription))
                return;

            if (ParentWindow.Evenement == null)
                ParentWindow.Evenement = new SaveableEvent();

            ParentWindow.Evenement.Name = EventName.Text;
            ParentWindow.Evenement.Description = EventDescription.Text;
            ParentWindow.Evenement.Addresse = EventAddresse.Text;
            ParentWindow.Evenement.Start = EventStart.Value.GetValueOrDefault();
            ParentWindow.Evenement.End = EventEnd.Value.GetValueOrDefault();

            Task.Run(() => Save());
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                db.Events.AddOrUpdate(ParentWindow.Evenement);
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "L'événement à bien été crée !" : "L'événement à bien été enregistré !");
                if (New) ParentWindow.ParentWindow.Add(ParentWindow.Evenement);
                else ParentWindow.ParentWindow.Update();
                ToggleBlocked(true);
                Saved = true;
            });
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

        public override bool CanClose() => Saved || !Saved && Validations.WillClose(true);

        public override bool CanBack() => Saved || Validations.WillClose(true);

        public override string CustomName => "EventMainPage";

    }

}