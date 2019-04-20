using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    ///     Interaction logic for MainEventManager.xaml
    /// </summary>
    public partial class EventMainPage
    {
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

        private bool New { get; } = true;
        private bool Saved { get; set; }
        private bool Blocked { get; set; }
        private EvenementManager ParentWindow { get; }

        public override string CustomName => "EventMainPage";

        private void ToggleBlocked(bool blocked)
        {
            EventName.IsEnabled = !blocked;
           // EventAddresse.IsEnabled = !blocked;
            EventDescription.IsEnabled = !blocked;
            EventStartButton.IsEnabled = !blocked;
            EventEndButton.IsEnabled = !blocked;
            EventSave.IsEnabled = !blocked;
            Blocage.IsChecked = blocked;
            Blocked = blocked;
        }

        private void FillTextBoxes()
        {
            EventName.Text = ParentWindow.Evenement.Name;
           // EventStart.Value = ParentWindow.Evenement.Start;
           // EventEnd.Value = ParentWindow.Evenement.End;
            EventDescription.Text = ParentWindow.Evenement.Description;
           // EventAddresse.Text = ParentWindow.Evenement.Address;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (Check(EventName)
				//|| Check(EventStart) || Check(EventEnd) || Check(EventAddresse) 
				|| Check(EventDescription))
                return;

            if (ParentWindow.Evenement == null)
                ParentWindow.Evenement = new SaveableEvent();

            ParentWindow.Evenement.Name = EventName.Text;
            ParentWindow.Evenement.Description = EventDescription.Text;
            //ParentWindow.Evenement.Address = EventAddresse.Text;
           // ParentWindow.Evenement.Start = EventStart.Value.GetValueOrDefault();
           // ParentWindow.Evenement.End = EventEnd.Value.GetValueOrDefault();

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
                MessageBox.Show(New ? "L'événement a bien été crée !" : "L'événement a bien été enregistré !");
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

        public override void Update()
        {
        }

        public override void Add<T>(T item)
        {
        }

        public override bool CanClose()
        {
            return Saved || !Saved && Validations.WillClose(true);
        }

        public override bool CanBack()
        {
            return Saved || Validations.WillClose(true);
        }
    }
}