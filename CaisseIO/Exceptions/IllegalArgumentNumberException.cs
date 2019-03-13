using System;
using System.Collections.Generic;
using System.Text;

namespace CaisseIO.Exceptions
{
    public class IllegalArgumentNumberException : Exception
    {
        public IllegalArgumentNumberException(int number, string name) : base(
            $"Le nombre d'arguments pour importer {name} est faux. Il en faut {number}.")
        {
        }
    }
}