using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Utils;
using CaisseServer;

namespace CaisseDesktop.Models.Windows
{
    public class StatisticsMainModel
    {
        private ICommand _clearInvoicesCommand;

        public ICommand ClearInvoicesCommand => _clearInvoicesCommand ??
                                                   (_clearInvoicesCommand =
                                                       new CommandHandler(ClearInvoices, true));

        public void ClearInvoices(object param)
        {
            if (!Validations.Ask("Etes-vous sûr de vouloir remettre à zero la base de données des commandes ?"))
                return;

            Task.Run(() => ClearInvoicesDB());
        }

        private void ClearInvoicesDB()
        {

            using (var db = new CaisseServerContext())
            {

                foreach (var operation in db.Operations)
                    db.Operations.Remove(operation);

                foreach (var invoice in db.Invoices)
                    db.Invoices.Remove(invoice);


                db.SaveChanges();

            }

            MessageBox.Show("La base de données a bien été mise à zero.");

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}