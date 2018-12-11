using System.Text;
using PrinterUtility;
using PrinterUtility.Enums;

namespace CaisseDesktop.Graphics.Print
{
    public class SalesReceipt : Ticket
    {
        public SalesReceipt()
            : base("\\\\PAUL\\EPSON TM-H6000IV Receipt")
        {
        }

        public override void Generate()
        {
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
            BytesValue = BytesValue.AddBytes(CutPage());
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