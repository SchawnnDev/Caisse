using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer.Export
{
    public interface IExportable
    {

        object[] Export();

    }
}
