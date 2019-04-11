using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CaisseLibrary.IO;
using CaisseLibrary.Print;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseLibrary
{
    public class Main
    {
        public static SaveableEvent ActualEvent { get; set; }
        public static Printer TicketPrinter { get; set; }
        public static BitmapManager BitmapManager { get; set; }
        public static List<SaveableArticle> Articles { get; set; }
        public static SaveableCheckout ActualCheckout { get; set; }
        public static SaveableCashier ActualCashier { get; set; }
        private static bool SessionOpen { get; set; }
        public static ReceiptTicket ReceiptTicket { get; set; }

        public static void Start()
        {
            ConfigFile.Init();
        }

        public static void ConfigureCheckout(string printerName)
        {
            ReceiptTicket = new ReceiptTicket(new TicketConfig(ActualEvent.Name, ActualEvent.Address, "67560 ROSHEIM",
                "+33788490372",
                "000111000555544"));
            BitmapManager = new BitmapManager(ActualEvent);
            TicketPrinter = new Printer(printerName);
            TicketPrinter.SetUp();

            using (var db = new CaisseServerContext())
            {
                Articles = db.Articles.Where(t => t.Type.Id == ActualCheckout.CheckoutType.Id).OrderBy(t => t.Position)
                    .ToList();
            }

            BitmapManager.Init(Articles);

            BitmapManager.ConvertEventLogo();

            TicketPrinter.SetUpImages(new List<ITicket> { });
        }

        public static void Reconfigure(string printerName)
        {
            TicketPrinter?.Close();
            BitmapManager = null;
            TicketPrinter = null;
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

        public static SaveableCashier Login(string login)
        {
            if (login.Length == 0 || SessionOpen) return null;

            SaveableCashier cashier;

            using (var db = new CaisseServerContext())
            {
                cashier = db.Cashiers.Any(t => t.Checkout.Id == ActualCheckout.Id && t.Login.Equals(login))
                    ? db.Cashiers.FirstOrDefault(t => t.Checkout.Id == ActualCheckout.Id && t.Login.Equals(login))
                    : null;
            }

            if (cashier != null)
                SessionOpen = true;

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