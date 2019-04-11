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
        public const int MAX_LINE_WIDTHS = 2;
        public const string NEW_LINE = "\n";
        public const string BOLD = "\u001b|bC";
        public const string CENTER = "\u001b|cA";
        public const string SEPARATOR = "---------------------------------------------";
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

        public long GetRecLineChars(PosPrinter printer, ref int[] RecLineChars)
        {
            long lRecLineChars = 0;
            long lCount;
            int i;

            // Calculate the element count.
            lCount = printer.RecLineCharsList.GetLength(0);

            if (lCount == 0)
            {
                lRecLineChars = 0;
            }
            else
            {
                if (lCount > MAX_LINE_WIDTHS) lCount = MAX_LINE_WIDTHS;

                for (i = 0; i < lCount; i++) RecLineChars[i] = printer.RecLineCharsList[i];

                lRecLineChars = lCount;
            }

            return lRecLineChars;
        }

        public void PrintMinimized(PosPrinter printer, string str)
        {
            var recLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
            var lRecLineCharsCount = GetRecLineChars(printer, ref recLineChars);
            if (lRecLineCharsCount >= 2)
            {
                printer.RecLineChars = recLineChars[1];
                printer.PrintNormal(PrinterStation.Receipt, str);
                printer.RecLineChars = recLineChars[0];
            }
            else
            {
                printer.PrintNormal(PrinterStation.Receipt, str);
            }
        }
    }
}