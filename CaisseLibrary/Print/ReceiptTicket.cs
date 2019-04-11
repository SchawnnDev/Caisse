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

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|N"
                                                        + Config.HostName + "\n");
            printer.PrintNormal(PrinterStation.Receipt, "\u001b|N"
                                                        + Config.Address + "\n");
            printer.PrintNormal(PrinterStation.Receipt, "\u001b|N"
                                                        + Config.PostalCodeCity + "\n");
            printer.PrintNormal(PrinterStation.Receipt, "\u001b|N"
                                                        + "TEL: " + Config.Telephone + "\n");


            //<<<step5>>--Start
            //Make 2mm speces
            //ESC|#uF = Line Feed
            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
            //<<<step5>>>-End

            /*
            lRecLineCharsCount = GetRecLineChars(ref RecLineChars);
            if (lRecLineCharsCount >= 2)
            {
                printer.RecLineChars = RecLineChars[1];
                printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
                printer.RecLineChars = RecLineChars[0];
            }
            else
            {
                printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
            }
            */

            string[] astritem = { "apples", "grapes", "bananas", "lemons", "oranges" };
            string[] astrprice = { "10.00", "20.00", "30.00", "40.00", "50.00" };
            //<<<step5>>>--Start
            //Make 5mm speces
            printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");

            //Print buying goods
            var total = 0.0;
            var strPrintData = "";
            for (var i = 0; i < astritem.Length; i++)
            {
                strPrintData = MakePrintString(printer.RecLineChars, astritem[i], "$" + astrprice[i]);

                printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

                total += 5d;
            }

            //Make 2mm speces
            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

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