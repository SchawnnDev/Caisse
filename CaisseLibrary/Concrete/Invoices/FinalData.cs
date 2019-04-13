using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseLibrary.Concrete.Invoices
{
    public struct FinalData
    {
        public List<SaveableOperation> Operations { get; set; }
        public SaveableConsign Consign { get; set; }

        public FinalData(List<SaveableOperation> operations, SaveableConsign consign)
        {
            Operations = operations;
            Consign = consign;
        }

    }
}
