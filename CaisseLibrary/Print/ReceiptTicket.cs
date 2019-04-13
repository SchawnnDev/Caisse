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

            ImageId = id;
        }

        public ITicket PrintWith(Invoice invoice)
        {
            Invoice = invoice;
            return this;
        }

        public override void Print(PosPrinter printer)
        {
            if (Invoice == null) return;

            /*
             * Logo
             */

            PrintBitmap(printer);

            /*
             *  Imprimer l'adresse
             */

            printer.PrintNormal(PrinterStation.Receipt, CENTER + Config.HostName + NEW_LINE);
            printer.PrintNormal(PrinterStation.Receipt, CENTER + Config.Address + NEW_LINE);
            printer.PrintNormal(PrinterStation.Receipt, CENTER + Config.PostalCodeCity + NEW_LINE);
            printer.PrintNormal(PrinterStation.Receipt, CENTER + "TEL: " + Config.GetTelephone + NEW_LINE);

            /*
             *  Caissier & date
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");

            PrintMinimized(printer,
                CENTER + $"CAISSIER : {Invoice.SaveableInvoice.Cashier.Id} • " + Invoice.SaveableInvoice.Date.ToString("dd/MM/yy HH:mm:ss") + NEW_LINE);

            /*
             *  Num facture
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");

            printer.PrintNormal(PrinterStation.Receipt,
                BOLD + CENTER + $"FACTURE N° : {Invoice.SaveableInvoice.Id}" + NEW_LINE);

            /*
             * 1. Separator
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|100uF");

            PrintMinimized(printer, CENTER + SEPARATOR + NEW_LINE);

            /*
             *  Items
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            foreach (var operation in Invoice.Operations)
            {
                printer.PrintNormal(PrinterStation.Receipt,
                    MakePrintString(printer.RecLineChars,
                        $"{operation.Amount} x {operation.Item.Name.ToUpper(Thread.CurrentThread.CurrentCulture)}",
                        $"{(operation.Item.Price * operation.Amount):F} €") + NEW_LINE);
            }

            if (Invoice.FinalData.Consign != null && Invoice.FinalData.Consign.Amount != 0)
            {
                printer.PrintNormal(PrinterStation.Receipt,
                    MakePrintString(printer.RecLineChars,
                        $"{Invoice.FinalData.Consign.Amount} x CONSIGNE",
                        $"{Invoice.FinalData.Consign.Amount:F} €") + NEW_LINE);
            }

            /*
             *  Total
             */

            var totalPrice = Invoice.CalculateTotalPrice();

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");

            printer.PrintNormal(PrinterStation.Receipt,
                CENTER + "\u001b|bC" + "\u001b|2C" + $"TOTAL : {totalPrice:F} €" + NEW_LINE);

            /*
             *  Payment Method & cashback
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            PrintMinimized(printer,
                CENTER +
                $"{Invoice.SaveableInvoice.PaymentMethod.Name.ToUpper()} : {Invoice.GivenMoney:F} EUR • RENDU : {Invoice.CalculateGivenBackChange():F} €" +
                NEW_LINE);
            //printer.PrintNormal(PrinterStation.Receipt,CENTER + $"{Invoice.PaymentMethod.Name.ToUpper()} : {Invoice.GivenMoney:F} EUR • RENDU : {Invoice.CalculateGivenBackChange():F} €" + NEW_LINE);

            /*
             * 2nd Separator
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            PrintMinimized(printer, CENTER + SEPARATOR + NEW_LINE);

            /*
             *  Tax
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            PrintMinimized(printer,
                CENTER + $"HT : {totalPrice:F} EUR • TVA 0% : 0,00 € • TTC : {totalPrice:F} €" + NEW_LINE); // TODO

            /*
             * Siret
             */

            if (Config.SiretActive)
            {
                printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

                PrintMinimized(printer, CENTER + $"SIRET : {Config.GetSiret} • TVA: Non soumis" + NEW_LINE); // TODO
            }
        }
    }
}