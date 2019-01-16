using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseLibrary.Concrete.Invoices
{
    public class Invoice
    {
        public decimal GivenMoney { get; set; }

        public SaveableInvoice SaveableInvoice { get; set; }

        public SaveablePaymentMethod PaymentMethod { get; set; }

        public List<SaveableOperation> Operations { get; set; }

        public Invoice(SaveableCashier cashier)
        {

            SaveableInvoice = new SaveableInvoice
            {
                Cashier = cashier
            };

            Operations = new List<SaveableOperation>();

        }

        public decimal CalculateTotalPrice() => Operations.Sum(t => t.FinalPrice());

        public decimal CalculateGivenBackChange() => GivenMoney - CalculateTotalPrice();

        public bool IsSomething() => Operations.Any();

        public void AddBuyableItem(SaveableItem item, int nb)
        {
            SetBuyableItem(item, Math.Max(0, nb));
        }

        public void RemoveBuyableItem(SaveableItem item, int nb)
        {
            SetBuyableItem(item, Math.Max(0, GetBuyableItemNumber(item) - nb));
        }

        public int GetBuyableItemNumber(SaveableItem item) => Operations.Any(t => t.Item.Id == item.Id)
            ? 0
            : Operations.First(t => t.Item.Id == item.Id).Amount;

        public void SetBuyableItem(SaveableItem item, int nb)
        {
            if (Operations.Any(t => t.Item.Id == item.Id))
            {
                if (nb == 0)
                {
                    Operations.Remove(Operations.FirstOrDefault(t => t.Item.Id == item.Id));
                }
                else
                {
                    Operations.First(t => t.Item.Id == item.Id).Amount = nb;
                }

                return;
            }

            Operations.Add(new SaveableOperation
            {
                Amount = nb,
                Item = item
            });
        }
    }
}