using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;

namespace CaisseLibrary.Concrete.Session
{
    public class CashierSession
    {
        public static SaveableCashier ActualCashier { get; set; }

        public static bool Login(int login)
        {
            if (login == 0) return false;

            using (var db = new CaisseServerContext())
            {
                if (!db.Cashiers.Any(t => t.TimeSlot.Day.Checkout.Id == CheckoutSession.ActualCheckout.Id && t.Login == login)) return false;
                ActualCashier = db.Cashiers.FirstOrDefault(t =>
                    t.TimeSlot.Day.Checkout.Id == CheckoutSession.ActualCheckout.Id && t.Login == login);
            }

            return true;
        }

        public static void Logout()
        {
            ActualCashier = null;
        }
    }
}