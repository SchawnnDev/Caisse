using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;

namespace CaisseDesktop.Models.Windows
{
    public class DisplayInvoiceModel
    {
        public SaveableInvoice Invoice { get; }

        private decimal _finalPrice;

        public decimal FinalPrice
        {
            get => _finalPrice;
            set
            {
                _finalPrice = value;
                OnPropertyChanged();
            }
        }

        public DisplayInvoiceModel(SaveableInvoice invoice)
        {
            Invoice = invoice;
            LoadOperations();
        }

        private void LoadOperations()
        {
            using (var db = new CaisseServerContext())
            {
                Operations = new ObservableCollection<SaveableOperation>(db.Operations
                    .Where(t => t.Invoice.Id == Invoice.Id).Include(t => t.Item).ToList());
            }

            FinalPrice = Operations.Sum(t => t.FinalPrice);
        }

        private ObservableCollection<SaveableOperation> _operations;

        public ObservableCollection<SaveableOperation> Operations
        {
            get => _operations;
            set
            {
                if (Equals(value, _operations)) return;

                _operations = value;
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