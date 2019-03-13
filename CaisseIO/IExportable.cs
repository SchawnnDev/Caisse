using System;
using System.Collections.Generic;
using System.Text;

namespace CaisseIO
{
    public interface IExportable
    {
        object[] Export();
    }
}