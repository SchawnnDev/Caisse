using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Models.Admin;
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
	        DataContext = new EventConfigModel(parentWindow.Evenement ?? new SaveableEvent(), parentWindow.Evenement == null);
	        ((EventConfigModel) DataContext).Dispatcher = Dispatcher;
        }

        private bool New { get; } = true;
        private bool Saved { get; set; }
        private bool Blocked { get; set; }
        private EvenementManager ParentWindow { get; }

        public override string CustomName => "EventMainPage";

        private void ToggleBlocked(bool blocked)
        {
          //  EventName.IsEnabled = !blocked;
            //EventAddresse.IsEnabled = !blocked;
         //   EventDescription.IsEnabled = !blocked;
         //   EventStart.IsEnabled = !blocked;
         //   EventEnd.IsEnabled = !blocked;
           // EventSave.IsEnabled = !blocked;
           // Blocage.IsChecked = blocked;
            Blocked = blocked;
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