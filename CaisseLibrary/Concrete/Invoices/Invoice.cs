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
        public DateTime Date { get; set; }

        public List<SaveableOperation> Operations { get; set; } 
            //if(!Operations.Any(t=>t.Item.Id == ))

        }
    }
}
