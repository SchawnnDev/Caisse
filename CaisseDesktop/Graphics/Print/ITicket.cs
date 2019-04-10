using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PointOfService;

namespace CaisseDesktop.Graphics.Print
{
    public interface ITicket
    {
        void SetImage(PosPrinter printer);

        void Print(PosPrinter printer);
    }
}