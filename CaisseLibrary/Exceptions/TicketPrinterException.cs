using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseLibrary.Exceptions
{
    public class TicketPrinterException : Exception
    {
        public TicketPrinterException(string message) : base($"Une erreur est survenue avec l'imprimante: {message}")
        {
        }
    }
}