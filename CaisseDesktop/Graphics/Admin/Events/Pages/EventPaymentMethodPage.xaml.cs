using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Checkouts;
using CaisseDesktop.Graphics.Admin.PaymentMethods;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Admin;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    ///     Interaction logic for EventCheckoutPage.xaml
    /// </summary>
    public partial class EventPaymentMethodPage
    {
        public EventPaymentMethodPage(EventManagerModel model)
        {
            InitializeComponent();
            ParentModel = model;

            New = model.SaveableEvent == null;

            Task.Run(Load);
        }

        private PaymentMethodModel PaymentMethodModel => PaymentMethodsGrid.DataContext as PaymentMethodModel;
        private bool New { get; }
        private EventManagerModel ParentModel { get; }

        public override string CustomName => "EventPaymentMethodPage";

        public override void Add<T>(T t)
        {
            PaymentMethodModel.PaymentMethods.Add(t as SaveablePaymentMethod);
        }

        public override void Update()
        {
            PaymentMethodsGrid.Items.Refresh();
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                PaymentMethodsGrid.DataContext = new PaymentMethodModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            var paymentMethodsCollection = new ObservableCollection<SaveablePaymentMethod>();

            if (!New)
                using (var db = new CaisseServerContext())
                {
                    paymentMethodsCollection = new ObservableCollection<SaveablePaymentMethod>(db.PaymentMethods
                        .Where(t => t.Event.Id == ParentModel.SaveableEvent.Id).ToList());
                }

            Dispatcher.Invoke(() =>
            {
                PaymentMethodModel.PaymentMethods = paymentMethodsCollection;
                Mouse.OverrideCursor = null;
            });
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveablePaymentMethod paymentMethod)
            {
	            new PaymentMethodManager(ParentModel, paymentMethod).ShowDialog();
            }
            else
                MessageBox.Show($"{btn} : le moyen de paiement n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
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