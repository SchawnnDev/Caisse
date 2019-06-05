using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer.Export.Exceptions
{
    public class IllegalArgumentNumberException : Exception
    {

        public IllegalArgumentNumberException(int id, string msg) : base(msg)
        {
        }

    }
}
