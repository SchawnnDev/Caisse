using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer.Export.Exceptions
{
    public class TypeNotRecognisedException : Exception
    {

        public TypeNotRecognisedException(string msg) : base(msg)
        {
        }

    }
}
