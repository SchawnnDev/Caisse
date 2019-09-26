using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CaisseDesktop.Models.Admin.Checkouts;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    ///     Interaction logic for CheckoutMainPage.xaml
    /// </summary>
    public partial class CheckoutMainPage
    {
        private CheckoutConfigModel Model => DataContext as CheckoutConfigModel;

        public CheckoutMainPage(CheckoutManagerModel parentModel)
        {
            InitializeComponent();

            using (var db = new CaisseServerContext())
            {
                var types = new ObservableCollection<SaveableCheckoutType>(db.CheckoutTypes
                    .OrderBy(e => e.Event.Id == parentModel.ParentModel.SaveableEvent.Id).ToList());
                var owners = new ObservableCollection<SaveableOwner>(db.Owners
                    .Where(t => t.Event.Id == parentModel.ParentModel.SaveableEvent.Id)
                    .OrderBy(e => e.LastLogin).Include(t => t.Event).ToList());

                DataContext = new CheckoutConfigModel(parentModel.SaveableCheckout, types, owners);
            }

            Model.Dispatcher = Dispatcher;
        }


        public override string CustomName => "CheckoutMainPage";

        public override void Update()
        {
        }

        public override void Add<T>(T item)
        {
        }

        public override bool CanClose() => !Model.CanSave || Model.CanSave && Validations.WillClose(true);

        public override bool CanBack() => !Model.CanSave || Validations.WillClose(true);
    }
}