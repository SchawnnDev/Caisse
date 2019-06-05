using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer.Export
{
    interface IImportable
    {
        void Import(object[] data);


    }
}
