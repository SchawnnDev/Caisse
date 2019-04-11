using System;
using System.Threading;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Exceptions;
using Microsoft.PointOfService;

namespace CaisseLibrary.Print
{
    public class ReceiptTicket : ITicket
    {
        private readonly TicketConfig Config;
        private Invoice Invoice { get; set; }

        public ReceiptTicket(TicketConfig config)
        {
            Config = config;
        }

        public override void SetImage(PosPrinter printer, int id)
        {
            if (!printer.CapRecBitmap) return;
            var bSetBitmapSuccess = false;

            var path = Main.BitmapManager.GetLogoPath;

            for (var iRetryCount = 0; iRetryCount < 5; iRetryCount++)
                try
                {
                    //Register a bitmap
                    printer.SetBitmap(id, PrinterStation.Receipt, path, printer.RecLineWidth,
                        PosPrinter.PrinterBitmapCenter);
                    bSetBitmapSuccess = true;
                    break;
                }
                catch (PosControlException pce)
                {
                    if (pce.ErrorCode == ErrorCode.Failure && pce.ErrorCodeExtended == 0 &&
                        pce.Message == "It is not initialized.")
                        Thread.Sleep(1000);
                }

            if (!bSetBitmapSuccess)
                throw new TicketPrinterException($"Impossible de mettre en place l'image {path}, après 5 essais.");
        }

        public ITicket PrintWith(Invoice invoice)
        {
            Invoice = invoice;
            return this;
        }

        public override void Print(PosPrinter printer)
        {
            // Print here.
            if (printer.CapRecBitmap) printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");

            /*
             *  Imprimer l'adresse
             */

            printer.PrintNormal(PrinterStation.Receipt, CENTER + Config.HostName + NEW_LINE);
            printer.PrintNormal(PrinterStation.Receipt, CENTER + Config.Address + NEW_LINE);
            printer.PrintNormal(PrinterStation.Receipt, CENTER + Config.PostalCodeCity + NEW_LINE);
            printer.PrintNormal(PrinterStation.Receipt, CENTER + "TEL: " + Config.Telephone + NEW_LINE);

            /*
             *  Caissier & date
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");

            PrintMinimized(printer, CENTER + "CAISSIER : 161 - " + DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + NEW_LINE);

            /*
             *  Num facture
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");

            printer.PrintNormal(PrinterStation.Receipt, BOLD + CENTER + "FACTURE N° : 14808" + NEW_LINE);

            /*
             * 1. Separator
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");

            PrintMinimized(printer,CENTER + SEPARATOR + NEW_LINE);

            /*
             *  Items
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            foreach (var operation in Invoice.Operations)
            {
                printer.PrintNormal(PrinterStation.Receipt, MakePrintString(printer.RecLineChars,$"{operation.Amount} {operation.Item.Name.ToUpper(Thread.CurrentThread.CurrentCulture)}", $"{(operation.Item.Price * operation.Amount)} €") + NEW_LINE);
            }

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            /*
             *  Total
             */
            const string ESC = "\u001B";
            const string GS = "\u001D";
            const string InitializePrinter = ESC + "@";
            const string BoldOn = ESC + "E" + "\u0001";
            const string BoldOff = ESC + "E" + "\0";
            const string DoubleOn = GS + "!" + "\u0011";  // 2x sized text (double-high + double-wide)
            const string DoubleOff = GS + "!" + "\0";

            printer.PrintNormal(PrinterStation.Receipt,   DoubleOn + "TOTAL : 2.00 €" + DoubleOff + NEW_LINE);

            return;
            string strPrintData;
            double total = 0;
            //Print the total cost
            strPrintData = MakePrintString(printer.RecLineChars, "Tax excluded."
                , "$" + total.ToString("F"));

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + strPrintData + "\n");

            strPrintData = MakePrintString(printer.RecLineChars, "Tax 5.0%", "$"
                                                                             + (total * 0.05)
                                                                             .ToString("F"));

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + strPrintData + "\n");

            strPrintData = MakePrintString(printer.RecLineChars / 2, "Total", "$"
                                                                              + (total * 1.05)
                                                                              .ToString("F"));

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C"
                                                                    + strPrintData + "\n");

            strPrintData = MakePrintString(printer.RecLineChars, "Customer's payment"
                , "$200.00");

            printer.PrintNormal(PrinterStation.Receipt
                , strPrintData + "\n");

            strPrintData = MakePrintString(printer.RecLineChars, "Change", "$"
                                                                           + (200.00 - total * 1.05)
                                                                           .ToString("F"));

            printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

            //Make 5mm speces
            printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");

        }

    }
}