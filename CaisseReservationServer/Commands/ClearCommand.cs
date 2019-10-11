using System;
using System.Collections.Generic;
using System.Text;
using CaisseReservationLibrary.Interfaces;

namespace CaisseReservationServer.Commands
{
    public class ClearCommand : ICommand
    {
        public void Process(string[] args)
        {
            Console.Clear();
        }

        public void PrintHelp()
        {
            Console.WriteLine("Clears the console.");
        }
    }
}
