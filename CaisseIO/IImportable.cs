using System;
using System.Collections.Generic;
using System.Text;

namespace CaisseIO
{
    public interface IImportable
    {
        void Import(object[] args);
    }
}