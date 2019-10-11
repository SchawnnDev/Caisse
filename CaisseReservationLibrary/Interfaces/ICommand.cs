using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseReservationLibrary.Interfaces
{
    public interface ICommand
    {

        string Name();

        void Process(string[] args);

        void PrintHelp();

    }
}
