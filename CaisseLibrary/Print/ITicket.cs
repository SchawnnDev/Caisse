using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PointOfService;

namespace CaisseLibrary.Print
{
    public abstract class ITicket
    {
        public abstract void SetImage(PosPrinter printer, int id);
        public abstract void Print(PosPrinter printer);

        public string MakePrintString(int iLineChars, string strBuf, string strPrice)
        {
            var tab = "";
            try
            {
                var iSpaces = iLineChars - (strBuf.Length + strPrice.Length);
                for (var j = 0; j < iSpaces; j++) tab += " ";
            }
            catch (Exception)
            {
                // ignored
            }

            return strBuf + tab + strPrice;
        }
    }
}