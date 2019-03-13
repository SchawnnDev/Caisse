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

        public static SaveableCashier Login(string login)
        {
            if (login.Length == 0) return null;

            using (var db = new CaisseServerContext())
            {
                if (!db.Cashiers.Any(t =>
                    t.TimeSlot.Checkout.Id == CheckoutSession.ActualCheckout.Id && t.Login.Equals(login))) return null;
                return db.Cashiers.FirstOrDefault(t =>
                    t.TimeSlot.Checkout.Id == CheckoutSession.ActualCheckout.Id && t.Login.Equals(login));
            }
        }

        public static void Logout()
        {
            ActualCashier = null;
        }
    }
}