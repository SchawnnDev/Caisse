using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Models.Windows;
using CaisseDesktop.Utils;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Statistics
{
    /// <summary>
    ///     Interaction logic for StatisticsMain.xaml
    /// </summary>
    public partial class StatisticsMain
    {
        private StatisticsMainModel Model => DataContext as StatisticsMainModel;

        public StatisticsMain()
        {
            InitializeComponent();

            DataContext = new StatisticsMainModel();

            LoadInfos();
        }

        public void LoadInfos()
        {
            int eventCount;
            int invoicesCount;
            decimal totalMoney;
            ObservableCollection<SaveableInvoice> invoices;

            using (var db = new CaisseServerContext())
            {
                eventCount = db.Events.Count();
                invoicesCount = db.Invoices.Count();
                totalMoney = 0; //db.Operations.Sum(t => t.Amount * t.Item.Price) + db.Consigns.Sum(t=>t.Amount); //todo: price
                var lastEventId = db.Events.OrderByDescending(t => t.Id).Select(t => t.Id).First();
                invoices = new ObservableCollection<SaveableInvoice>(db.Invoices
                    .Where(t => t.Cashier.Checkout.CheckoutType.Event.Id == lastEventId).Include(t => t.Cashier)
                    .Include(t => t.PaymentMethod).OrderBy(t => t.Date).ToList());
            }

            Model.EventCount = eventCount;
            Model.InvoicesCount = invoicesCount;
            Model.TotalMoney = totalMoney;
            Model.Invoices = invoices;
        }

        private void DisplayInvoice_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (!(btn?.DataContext is SaveableInvoice invoice)) return;

	        new DisplayInvoice(invoice).ShowDialog();

        }

        private void DeleteInvoice_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (!(btn?.DataContext is SaveableInvoice invoice)) return;

            if (!Validations.Ask("Etes-vous sûr de vouloir de supprimer cette commande ?"))
                return;

            Task.Run(() => DeleteInvoice(invoice));
        }

        private void DeleteInvoice(SaveableInvoice invoice)
        {
            using (var db = new CaisseServerContext())
            {
             //   db.Operations.RemoveRange(db.Operations.Where(t => t.Invoice.Id == invoice.Id).ToList());
	           db.Invoices.Attach(invoice);
                db.Invoices.Remove(invoice);
                db.SaveChanges();
            }

            MessageBox.Show("La commande a bien été supprimée");

        }

    }
}