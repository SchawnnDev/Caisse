using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseLibrary
{
    public class Main
    {

        public static SaveableEvent ActualEvent { get; set; }

        public static void Start()
        {

            ConfigFile.Init();

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
                    .Where(t => t.SaveableEvent.Id == eventId).OrderBy(t => t.CheckoutType.Id).ToList();
            }
        }
    }
}
