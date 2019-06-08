using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Models.Windows;
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


            Task.Run(LoadInfos);
        }

        public void LoadInfos()
        {
            var eventCount = 0;
            var invoicesCount = 0;
            var totalMoney = 0m;

            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                eventCount = db.Events.Count();
                invoicesCount = db.Invoices.Count();
                totalMoney = db.Operations.Sum(t => t.Amount * t.Item.Price);
            }

            Dispatcher.Invoke(() =>
            {
                Model.EventCount = eventCount;
                Model.InvoicesCount = invoicesCount;
                Model.TotalMoney = totalMoney;
                Mouse.OverrideCursor = null;
            });


        }
    }
}