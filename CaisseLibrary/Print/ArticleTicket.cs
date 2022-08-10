using System;
using System.Threading;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Exceptions;
using CaisseServer.Items;
using Microsoft.PointOfService;

namespace CaisseLibrary.Print
{
    public class ArticleTicket : ITicket
    {
        private Invoice Invoice { get; set; }
        public SaveableArticle Article { get; }

        public ArticleTicket(SaveableArticle article)
        {
            //Config = config;
            Article = article;
        }

        public override void SetImage(PosPrinter printer, int id)
        {/*
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

            ImageId = id; */
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

            //PrintBitmap(printer);

            printer.PrintBitmap(PrinterStation.Receipt, Main.BitmapManager.GetBitmapPath(Article), printer.RecLineWidth / 2
               , PosPrinter.PrinterBitmapCenter);

            // PrintBitmap(printer);

            /*
             *  Résumé
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");

            printer.PrintNormal(PrinterStation.Receipt,
                CENTER + "\u001b|bC" + "\u001b|2C" + $"1 x {Article.Name.ToUpper()}" + NEW_LINE);

            /*
             *  Informations
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            PrintMinimized(printer, CENTER + "Ce ticket est uniquement échangeable" + NEW_LINE);

            PrintMinimized(printer, CENTER + "contre l'article précisé ci-dessus." + NEW_LINE);

            /*
             *  Informations supplémentaires
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

            PrintMinimized(printer,
                CENTER + $"Facture n°{Invoice.SaveableInvoice.Id} • {Invoice.SaveableInvoice.Date:dd/MM/yy HH:mm}" + NEW_LINE);

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");
        }
    }
}