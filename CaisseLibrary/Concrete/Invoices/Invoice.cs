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
        public SaveableInvoice SaveableInvoice { get; set; }

        public List<SaveableOperation> Operations { get; set; }

        public SaveableConsign Consign { get; set; }

        public FinalData FinalData { get; set; }

        public Invoice(SaveableInvoice invoice, SaveableConsign consign, List<SaveableOperation> operations)
        {
            SaveableInvoice = invoice;
            Consign = consign;
            Operations = operations;
        }

        public void FinalizeInvoice()
        {
            SaveableInvoice.Date = DateTime.Now;
            var operationsToSave = new List<SaveableOperation>();

            foreach (var operation in Operations)
            {
                operationsToSave.Add(new SaveableOperation
                {
                    Amount = operation.Amount,
                    Item = operation.Item,
                    Invoice = SaveableInvoice
                });
            }

            /**
             *  idk if its better to do it here. // todo:
             */

            var gratisOperations = new List<SaveableOperation>();

            foreach (var @event in Main.ArticleEvents)
            {
                foreach (var operation in operationsToSave)
                {
                    if (@event.Item.Id != operation.Item.Id || operation.Amount == 0) continue;
                    if (operation.Amount % @event.NeededAmount != 0) continue;

                    gratisOperations.Add(new SaveableOperation
                    {
                        Amount = operation.Amount / @event.NeededAmount,
                        IsEventItem = true,
                        Item = new SaveableArticle
                        {
                            Id = operation.Item.Id,
                            ImageSrc = operation.Item.ImageSrc,
                            Name = $"{operation.Item.Name} (offert)"
                        }
                    });
                }
            }

            operationsToSave.AddRange(gratisOperations);

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
                    ticketList.Add(operation.IsEventItem
                        ? new ArticleTicket(operation.Item).PrintWith(this)
                        : Main.GetArticleTicket(operation.Item).PrintWith(this));
                }
            }

            // Print consign tickets
            for (var i = 0; i < Consign.Amount; i++)
                ticketList.Add(Main.ConsignTicket.PrintWith(this));

            Main.TicketPrinter.Print(ticketList);
        }

        public decimal CalculateTotalArticlesPrice() => Operations.Sum(t => t.FinalPrice);
        public decimal CalculateTotalPrice() => CalculateTotalArticlesPrice() + Consign.Amount;
        public decimal CalculateGivenBackChange() => Math.Max(0, SaveableInvoice.GivenMoney - CalculateTotalPrice());
	    public bool HasConsigns() => Consign != null && Consign.Amount > 0;

        public void Save()
        {
            using (var db = new CaisseServerContext())
            {
                SaveableInvoice.Cashier = Main.ActualCashier;
                SaveableInvoice.PaymentMethod = Main.LiquidPaymentMethod;

                db.Cashiers.Attach(SaveableInvoice.Cashier);
                db.PaymentMethods.Attach(SaveableInvoice.PaymentMethod);

                foreach (var operation in FinalData.Operations)
                {
                    if (operation.IsEventItem) continue;
                    db.Articles.Attach(operation.Item);
                }

                db.Invoices.Add(SaveableInvoice);

                if (FinalData.Consign != null)
                    db.Consigns.Add(FinalData.Consign);

                foreach (var operation in FinalData.Operations)
                {
                    if (operation.IsEventItem) continue;
                    db.Operations.Add(operation);
                }

                db.SaveChanges();
            }
        }
    }
}