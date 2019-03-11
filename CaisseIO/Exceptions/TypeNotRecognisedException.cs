using System;
using System.Collections.Generic;
using System.Text;

namespace CaisseIO.Exceptions
{
    public class TypeNotRecognisedException : Exception
    {

        public TypeNotRecognisedException(string nom) : base($"Le type n'est pas reconnu, il doit être un {nom}.")
        {

        }


    }
}
