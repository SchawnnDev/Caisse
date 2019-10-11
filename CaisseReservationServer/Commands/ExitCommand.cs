using System;
using System.Collections.Generic;
using System.Text;
using CaisseReservationLibrary.Interfaces;

namespace CaisseReservationServer.Commands
{
    public class ExitCommand : ICommand
    {
        public void Process(string[] args)
        {
            Environment.Exit(0);
        }

        public void PrintHelp()
        {
            Console.WriteLine("Stops the server.");
        }
    }
}
