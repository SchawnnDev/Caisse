using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseDesktop.Graphics.Print;
using CaisseServer;
using Microsoft.PointOfService;

namespace CaisseDesktop.Client
{
    public class ClientMain
    {

        public static Printer TicketPrinter { get; set; }
        public static SaveableCheckout ActualCheckout { get; set; }
        public static SaveableCashier ActualCashier { get; set; }

        public static SaveableCashier Login(string login)
        {
            if (login.Length == 0) return null;

            using (var db = new CaisseServerContext())
            {
                // if (!db.Cashiers.Any(t =>
                //     t.TimeSlot.Checkout.Id == CheckoutSession.ActualCheckout.Id && t.Login.Equals(login))) return null;
                //return db.Cashiers.FirstOrDefault(t =>
                //        t.TimeSlot.Checkout.Id == CheckoutSession.ActualCheckout.Id && t.Login.Equals(login));
            }

            return null; /// TODO
        }

        public static void Logout()
        {
            ActualCashier = null;
        }

        public static void Init()
        {
            TicketPrinter = new Printer("TicketsPrinter");
            Task.Run(() => CreateTickets());

        }


        private static void CreateTickets()
        {

            using (var db = new CaisseServerContext())
            {

            }


        }

    }
}
