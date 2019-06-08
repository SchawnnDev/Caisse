using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseLibrary.Concrete.Invoices;
using CaisseServer.Items;
using Microsoft.PointOfService;

namespace CaisseLibrary.Print
{
    public class ConsignTicket : ITicket
    {
        private Invoice Invoice { get; set; }
        public string ImagePath { get; }

        public ConsignTicket(string imagePath)
        {
            ImagePath = imagePath;
        }

        public override void SetImage(PosPrinter printer, int id) { }

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

            printer.PrintBitmap(PrinterStation.Receipt, ImagePath, printer.RecLineWidth / 2
                , PosPrinter.PrinterBitmapCenter);

            // PrintBitmap(printer);

            /*
             *  Résumé
             */

            printer.PrintNormal(PrinterStation.Receipt, "\u001b|250uF");

            printer.PrintNormal(PrinterStation.Receipt,
                CENTER + "\u001b|bC" + "\u001b|2C" + $"1 x CONSIGNE" + NEW_LINE);

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
                CENTER + $"Facture n°{Invoice.SaveableInvoice.Id} • {Invoice.SaveableInvoice.Date:dd/MM/yy HH:mm:ss}" + NEW_LINE);
        }
    }
}
