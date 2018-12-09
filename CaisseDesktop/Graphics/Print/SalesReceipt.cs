using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterUtility;
using PrinterUtility.EscPosEpsonCommands;

namespace CaisseDesktop.Graphics.Print
{
    public class SalesReceipt : Ticket
    {

        public SalesReceipt()
            : base("\\\\beru\\EPSON TM-H6000IV Receipt")
        {
        }

        public override void Generate()
        {
            var logo = GetLogo("Resources/Images/logo-receipt.png");
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Alignment.Center());
            BytesValue = PrintExtensions.AddBytes(BytesValue,logo);
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.CharSize.DoubleWidth6());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.FontSelect.FontA());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Alignment.Center());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Title\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.CharSize.DoubleWidth4());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Sub Title\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.CharSize.Nomarl());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Invoice\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Alignment.Left());
            BytesValue = PrintExtensions.AddBytes(BytesValue, CutPage());
            return;
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Invoice No. : 12345\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Date        : 12/12/2015\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Itm                      Qty      Net   Total\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, $"{"item 1",-40}{12,6}{11,9}{144.00,9:N2}\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, $"{"item 2",-40}{12,6}{11,9}{144.00,9:N2}\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Alignment.Right());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Total\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("288.00\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Lf());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Alignment.Center());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.CharSize.DoubleHeight6());
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.BarCode.Code128("12345"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.QrCode.Print("12345", PrinterUtility.Enums.QrCodeSize.Grande));
            BytesValue = PrintExtensions.AddBytes(BytesValue, "-------------------Thank you for coming------------------------\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, EscPosEpson.Alignment.Left());
            BytesValue = PrintExtensions.AddBytes(BytesValue, CutPage());
        }

    }
}
