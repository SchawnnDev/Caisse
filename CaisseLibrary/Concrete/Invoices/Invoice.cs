using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseLibrary.Exceptions;
using CaisseLibrary.Print;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseLibrary.Concrete.Invoices
{
    public class Invoice
    {
        public decimal GivenMoney { get; set; }

        public SaveableInvoice SaveableInvoice { get; set; }

        public List<SaveableOperation> Operations { get; set; }

        public SaveableConsign Consign { get; set; }

        public FinalData FinalData { get; set; }

        public Invoice()
        {
            SaveableInvoice = new SaveableInvoice();

            Consign = new SaveableConsign
            {
                Amount = 0,
                Invoice = SaveableInvoice
            };

            Operations = new List<SaveableOperation>();

            foreach (var article in Main.Articles)
            {
                Operations.Add(new SaveableOperation
                {
                    Amount = 0,
                    Invoice = SaveableInvoice,
                    Item = article
                });
            }
        }

        public void SetArticleCount(SaveableArticle article, int count)
        {
            Operations.Single(t => t.Item.Id == article.Id).Amount = count;
        }

        public void FinalizeInvoice()
        {
            SaveableInvoice.Date = DateTime.Now;
            var operationsToSave = new List<SaveableOperation>();

            foreach (var operation in Operations)
            {
                if (operation.Amount == 0) continue;
                operationsToSave.Add(operation);
            }

            FinalData = new FinalData
            {
                Operations = operationsToSave,
                Consign = Consign.Amount > 0 ? Consign : null
            };
        }

        public void Print(bool receiptTicket)
        {
            var ticketList = new List<ITicket>();

            if (receiptTicket) ticketList.Add(Main.ReceiptTicket.PrintWith(this));

            foreach (var operation in FinalData.Operations)
            {
                for (var i = 0; i < operation.Amount; i++)
                {
                    ticketList.Add(Main.GetArticleTicket(operation.Item).PrintWith(this));
                }
            }

            Main.TicketPrinter.Print(ticketList);
        }

        public decimal CalculateTotalArticlesPrice() => Operations.Sum(t => t.FinalPrice());

        public decimal CalculateTotalPrice() => CalculateTotalArticlesPrice() + Consign.Amount;

        public decimal CalculateGivenBackChange() => Math.Max(0, GivenMoney - CalculateTotalPrice());

        public bool IsSomething() => Operations.Any() && Operations.Count(t => t.Amount > 0) > 0;

        public void Save()
        {
            using (var db = new CaisseServerContext())
            {
                SaveableInvoice.Cashier = Main.ActualCashier;
                SaveableInvoice.GivenMoney = GivenMoney;
                SaveableInvoice.PaymentMethod = Main.LiquidPaymentMethod;

                db.Cashiers.Attach(SaveableInvoice.Cashier);
                db.PaymentMethods.Attach(SaveableInvoice.PaymentMethod);

                foreach (var operation in FinalData.Operations)
                {
                    db.Articles.Attach(operation.Item);
                }

                db.Invoices.Add(SaveableInvoice);

                if (FinalData.Consign != null)
                    db.Consigns.Add(FinalData.Consign);

                foreach (var operation in FinalData.Operations)
                {
                    db.Operations.Add(operation);
                }

                db.SaveChanges();
            }
        }
    }
}