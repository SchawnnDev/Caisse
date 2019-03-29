using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CaisseLibrary.Exceptions;
using CaisseLibrary.Interfaces;
using Microsoft.PointOfService;

namespace CaisseLibrary.Concrete.Print
{
    public class TicketPrinter : ITicketPrinter
    {
        private PosPrinter m_Printer;
        private string LogicalName { get; set; }
        private bool Image { get; set; }
        private string ImagePath { get; set; }

        public TicketPrinter(string logicalName)
        {
            LogicalName = logicalName;
        }

        public TicketPrinter(string logicalName, string imagePath)
        {
            LogicalName = logicalName;
            Image = true;
            ImagePath = imagePath;
        }

        public void Load()
        {
            //Current Directory Path
            //var strCurDir = Directory.GetCurrentDirectory();

            //var strFilePath = strCurDir.Substring(0, strCurDir.LastIndexOf("bin"));

            //strFilePath += "Logo.bmp";

            //Create PosExplorer
            var posExplorer = new PosExplorer();

            DeviceInfo deviceInfo = null;

            //<<<step10>>>--Start
            try
            {
                deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter, LogicalName);
            }
            catch (Exception)
            {
                throw new TicketPrinterException("Failed to get device information.");
            }

            try
            {
                m_Printer = (PosPrinter) posExplorer.CreateInstance(deviceInfo);
            }
            catch (Exception)
            {
                throw new TicketPrinterException("Failed to create instance.");
            }

            //<<<step9>>>--Start	
            //Register OutputCompleteEventHandler. AddOutputCompleteEvent(m_Printer);
            //<<<step9>>>--End

            //<<<step10>>>--Start	
            //Register OutputCompleteEventHandler. AddErrorEvent(m_Printer);

            //Register OutputCompleteEventHandler.  AddStatusUpdateEvent(m_Printer);


            try
            {
                //Open the device
                m_Printer.Open();
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException("Failed to open the device.");
            }

            try
            {
                //Get the exclusive control right for the opened device.
                //Then the device is disable from other application.
                m_Printer.Claim(1000);
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException("Failed to claim the device.");
            }

            try
            {
                //Enable the device.
                m_Printer.DeviceEnabled = true;
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException("Disable to use the device.");
            }

            try
            {
                //<<<step3>>>--Start
                //Output by the high quality mode
                m_Printer.RecLetterQuality = true;

                // Even if using any printers, 0.01mm unit makes it possible to print neatly.
                m_Printer.MapMode = MapMode.Metric;
            }
            catch (PosControlException)
            {
            }

            if (m_Printer.CapRecBitmap)
            {
                var bSetBitmapSuccess = false;
                for (var iRetryCount = 0; iRetryCount < 5; iRetryCount++)
                    try
                    {
                        //<<<step5>>>--Start
                        //Register a bitmap
                        m_Printer.SetBitmap(1, PrinterStation.Receipt,
                            ImagePath, m_Printer.RecLineWidth / 2,
                            PosPrinter.PrinterBitmapCenter);
                        //<<<step5>>>--End
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
                    throw new TicketPrinterException("Failed to set bitmap.");
            }
        }
    }
}