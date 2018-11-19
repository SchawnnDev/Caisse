using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer
{
    public class CaisseServerContext : DbContext
    {

        public CaisseServerContext() : base("name=ConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
