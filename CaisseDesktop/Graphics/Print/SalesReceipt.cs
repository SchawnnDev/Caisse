using System;
using System.Text;
using com.epson.pos.driver;
using CaisseLibrary.Concrete.Invoices;
using PrinterUtility;

namespace CaisseDesktop.Graphics.Print
{
    public class SalesReceipt : Ticket
    {
        private Invoice PrintableInvoice { get; }

        public SalesReceipt(Invoice invoice)
            : base("\\\\PAUL\\EPSON TM-H6000IV Receipt")
        {
            PrintableInvoice = invoice;
        }

        private void GenerateHeader()
        {
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.Nomarl());
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Center());
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Association FANABRIQUES\n"));
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("14 avenue Foch\n"));
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("67560 ROSHEIM\n"));
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("TEL : N.C.\n\n"));
            //
            BytesValue =
                BytesValue.AddBytes(Encoding.ASCII.GetBytes(
                    $"CAISSIER : {PrintableInvoice.SaveableInvoice.Cashier.Id} - {PrintableInvoice.SaveableInvoice.Date}\n\n"));
            BytesValue =
                BytesValue.AddBytes(Encoding.ASCII.GetBytes($"FACTURE : {PrintableInvoice.SaveableInvoice.Id}\n\n"));
        }

        private void GenerateFooter()
        {
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes($"RT : {PrintableInvoice.CalculateTotalPrice()}E - TVA 0% : 0.00E - TTC : {PrintableInvoice.CalculateTotalPrice()} E\n"));
        }

        private void GenerateBody()
        {
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Left());

            foreach (var operation in PrintableInvoice.Operations)
            {
                BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes($"{operation.Amount} {operation.Item.Name}                                     {operation.FinalPrice()}E\n\n"));
            }

            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Center());
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.DoubleHeight2());
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.DoubleWidth2());
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes($"TOTAL : {PrintableInvoice.CalculateTotalPrice()}E\n\n"));
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.Nomarl());
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes($"{PrintableInvoice.SaveableInvoice.PaymentMethod.Name} : {PrintableInvoice.GivenMoney} EUR - RENDU : {PrintableInvoice.CalculateGivenBackChange()} EUR\n"));
        }


        public override void Generate()
        {
            var logo = GetLogo("Resources/Images/logo-receipt.png");

            BytesValue = BytesValue.AddBytes(logo);

            GenerateHeader();

            BytesValue = BytesValue.AddBytes(EscPosEpson.Separator());

            GenerateBody();

            BytesValue = BytesValue.AddBytes(EscPosEpson.Separator());

            GenerateFooter();

            BytesValue = BytesValue.AddBytes(CutPage());


            /*  return;
            //var logo = GetLogo("Resources/Images/logo_brique.png");
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Center());
            //BytesValue = BytesValue.AddBytes(logo);
            BytesValue = BytesValue.AddBytes(EscPosEpson.Separator());
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.DoubleWidth6());
            BytesValue = BytesValue.AddBytes(EscPosEpson.FontSelect.FontC());
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Center());
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Title\n"));
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.DoubleWidth4());
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Sub Title\n"));
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.Nomarl());
            BytesValue = BytesValue.AddBytes(EscPosEpson.Separator());
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Invoice\n"));
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Left());
            BytesValue = BytesValue.AddBytes(CutPage()); 
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Invoice No. : 12345\n"));
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Date        : 12/12/2015\n"));
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Itm                      Qty      Net   Total\n"));
            BytesValue = BytesValue.AddBytes(EscPosEpson.Separator());
            BytesValue = BytesValue.AddBytes($"{"item 1",-40}{12,6}{11,9}{144.00,9:N2}\n");
            BytesValue = BytesValue.AddBytes($"{"item 2",-40}{12,6}{11,9}{144.00,9:N2}\n");
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Right());
            BytesValue = BytesValue.AddBytes(EscPosEpson.Separator());
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("Total\n"));
            BytesValue = BytesValue.AddBytes(Encoding.ASCII.GetBytes("288.00\n")); 
            BytesValue = BytesValue.AddBytes(EscPosEpson.Separator());
            BytesValue = BytesValue.AddBytes(EscPosEpson.Lf());
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Center());
            BytesValue = BytesValue.AddBytes(EscPosEpson.CharSize.DoubleHeight6()); 
            BytesValue = BytesValue.AddBytes(EscPosEpson.BarCode.Code128("12345"));
            BytesValue = BytesValue.AddBytes(EscPosEpson.QrCode.Print("SALUT TOM", QrCodeSize.Gigante));
            BytesValue = BytesValue.AddBytes("-------------------Thank you for coming------------------------\n");
            BytesValue = BytesValue.AddBytes(EscPosEpson.Alignment.Left());
            BytesValue = BytesValue.AddBytes(CutPage()); */
        }

        /*
        public override void Generate()
        {
            BytesValue = BytesValue.AddBytes(GetLogo("C:/Users/Meyer/Pictures/gamin.png"));
            //BytesValue = BytesValue.AddBytes(EscPosEpson.QrCode.Print("BITTE MACH DEINE HAUSAUFGABEN SPAETER", QrCodeSize.Gigante));
            BytesValue = BytesValue.AddBytes(CutPage());
        }*/
    }
}