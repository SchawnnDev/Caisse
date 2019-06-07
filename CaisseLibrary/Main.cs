using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.IO;
using CaisseLibrary.Print;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseLibrary
{
    public class Main
    {
        public static SaveablePaymentMethod LiquidPaymentMethod { get; set; }
        public static SaveableEvent ActualEvent { get; set; }
        public static Printer TicketPrinter { get; set; }
        public static BitmapManager BitmapManager { get; set; }
        public static List<SaveableArticle> Articles { get; set; }
        public static SaveableCheckout ActualCheckout { get; set; }
        public static SaveableCashier ActualCashier { get; set; }
        private static bool SessionOpen { get; set; }
        public static ReceiptTicket ReceiptTicket { get; set; }
        public static List<ArticleTicket> ArticleTickets { get; set; }
        public static ConsignTicket ConsignTicket { get; set; }
       // public static Invoice ActualInvoice { get; set; }

        public static void Start()
        {
            ConfigFile.Init();
        }

        public static void ConfigureCheckout(string printerName)
        {
            ReceiptTicket = new ReceiptTicket(new TicketConfig(ActualEvent.Name, ActualEvent.Address, ActualEvent.PostalCodeCity, ActualEvent.Telephone, ActualEvent.Siret));
            BitmapManager = new BitmapManager(ActualEvent);
            ArticleTickets = new List<ArticleTicket>();

            using (var db = new CaisseServerContext())
            {
                Articles = db.Articles.Where(t => t.Type.Id == ActualCheckout.CheckoutType.Id).OrderBy(t => t.Position)
                    .ToList();

                if (db.PaymentMethods.Any())
                {
                    LiquidPaymentMethod = db.PaymentMethods.Include(t=>t.Event).Single();
                }
                else
                {
                    var paymentMethod = new SaveablePaymentMethod
                    {
                        Event = ActualEvent,
                        MinFee = 0,
                        Name = "Espèces",
                        Type = "Liquid"
                    };
                    db.Events.Attach(ActualEvent);
                    db.PaymentMethods.Add(paymentMethod);
                    db.SaveChanges();
                    LiquidPaymentMethod = paymentMethod;
                }

            }

            BitmapManager.Init(Articles);

            BitmapManager.ConvertEventLogo();

            /*
             *  Setup printer images
             */

            var imagesToSetUp = new List<ITicket> {ReceiptTicket};

            foreach (var article in Articles)
            {
                var articleTicket = new ArticleTicket(article);
                ArticleTickets.Add(articleTicket);
                imagesToSetUp.Add(articleTicket);
            }

            // consign ticket

            ConsignTicket = new ConsignTicket(Path.Combine(Directory.GetCurrentDirectory(), "Ressources" + Path.PathSeparator + "Images" + Path.PathSeparator + "cup.jpg"));

			// setup printer

	        TicketPrinter = new Printer(printerName);
	        TicketPrinter.SetUp();

			// setup images

	        TicketPrinter.SetUpImages(imagesToSetUp);

		}

        public static void Reconfigure(string printerName)
        {
            TicketPrinter?.Close();
            BitmapManager = null;
            TicketPrinter = null;
            ArticleTickets = null;
            ReceiptTicket = null;
            ConfigureCheckout(printerName);
        }

        public static List<SaveableEvent> LoadEvents()
        {
            using (var db = new CaisseServerContext())
            {
                return db.Events.OrderByDescending(t => t.Start).ToList();
            }
        }

        public static List<SaveableCheckout> LoadCheckouts(int eventId)
        {
            using (var db = new CaisseServerContext())
            {
                return db.Checkouts.Include(t => t.CheckoutType)
                    .Where(t => t.CheckoutType.Event.Id == eventId).OrderBy(t => t.CheckoutType.Id).ToList();
            }
        }

        public static ArticleTicket GetArticleTicket(SaveableArticle article)
        {
            foreach (var ticket in ArticleTickets)
            {
                if (ticket.Article == null || ticket.Article.Id != article.Id) continue;
                return ticket;
            }

            return null;
        }

        public static SaveableCashier Login(string login)
        {
            if (login.Length == 0 || SessionOpen) return null;

            SaveableCashier cashier;

            using (var db = new CaisseServerContext())
            {
                cashier = db.Cashiers.Any(t => t.Checkout.Id == ActualCheckout.Id && t.Login.Equals(login))
                    ? db.Cashiers.FirstOrDefault(t => t.Checkout.Id == ActualCheckout.Id && t.Login.Equals(login))
                    : null;

                if (cashier != null)
                {
                    db.Cashiers.Attach(cashier);
                    cashier.WasHere = true;
                    cashier.LastActivity = DateTime.Now;
                    db.Entry(cashier).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    SessionOpen = true;
                }


            }



            return cashier;
        }

        public static void Logout()
        {
            if (!SessionOpen) return;
            ActualCashier = null;
            SessionOpen = false;
        }
    }
}