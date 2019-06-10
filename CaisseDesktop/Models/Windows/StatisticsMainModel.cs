using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Utils;
using CaisseServer;
using static System.Windows.MessageBox;

namespace CaisseDesktop.Models.Windows
{
    public class StatisticsMainModel
    {
        private ICommand _clearInvoicesCommand;

        public ICommand ClearInvoicesCommand => _clearInvoicesCommand ??
                                                (_clearInvoicesCommand =
                                                    new CommandHandler(ClearInvoices, true));


        public void DeleteInvoice(object param)
        {
            if (!(param is SaveableInvoice invoice)) return;

            if (!Validations.Ask("Etes-vous sûr de vouloir de supprimer cette commande ?"))
                return;

            Task.Run(() => DeleteInvoice(invoice));

        }

        private void DeleteInvoice(SaveableInvoice invoice)
        {
            using (var db = new CaisseServerContext())
            {
                db.Operations.RemoveRange(db.Operations.Where(t => t.Invoice.Id == invoice.Id).ToList());
                db.Invoices.Remove(invoice);
                db.SaveChanges();
            }

            Show("La ligne a bien été supprimée");

        }

        public void DisplayInvoice(object param)
        {
            if (!(param is SaveableInvoice invoice)) return;
        }

        public void ClearInvoices(object param)
        {
            if (!Validations.Ask("Etes-vous sûr de vouloir remettre à zero la base de données des commandes ?"))
                return;

            Task.Run(()=>ClearInvoicesDB());
        }

        private void ClearInvoicesDB()
        {
            using (var db = new CaisseServerContext())
            {
                db.Operations.RemoveRange(db.Operations.ToList());
                db.Invoices.RemoveRange(db.Invoices.ToList());
                db.SaveChanges();
            }

            Show("La base de données a bien été mise à zéro.");
            Invoices.Clear();
        }

        private int _eventCount;

        public int EventCount
        {
            get => _eventCount;
            set
            {
                _eventCount = value;
                OnPropertyChanged();
            }
        }

        private int _invoicesCount;

        public int InvoicesCount
        {
            get => _invoicesCount;
            set
            {
                _invoicesCount = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalMoneyCount;

        public decimal TotalMoney
        {
            get => _totalMoneyCount;
            set
            {
                _totalMoneyCount = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SaveableInvoice> _invoices;

        public ObservableCollection<SaveableInvoice> Invoices
        {
            get => _invoices;
            set
            {
                if (Equals(value, _invoices)) return;

                _invoices = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}