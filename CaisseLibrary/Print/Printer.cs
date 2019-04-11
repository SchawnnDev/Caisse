using System;
using System.Collections.Generic;
using System.Threading;
using CaisseLibrary.Exceptions;
using Microsoft.PointOfService;

namespace CaisseLibrary.Print
{
    public class Printer
    {
        public PosPrinter PosPrinter { get; set; }
        private string PrinterName { get; set; }

        /**
         *  On part sur la logique, que le printer soit crée en fin de commande, quand on veut imprimer. Si aucune latence.
         *  Sinon, on met un printer permanent qui se renouvelle tout les X minutes.
         *  Les Logos sont enregistrés 1 par 1 au démarrage de l'application => donc la deuxième technique est la technique que l'on va utiliser.
         */

        public Printer(string printerName)
        {
            PrinterName = printerName;
        }

        public void SetUp()
        {
            var posExplorer = new PosExplorer();
            DeviceInfo deviceInfo;

            try
            {
                deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter, PrinterName);
            }
            catch (Exception)
            {
                throw new TicketPrinterException(
                    $"Impossible d'établir une connexion avec l'imprimante {PrinterName}.");
            }

            try
            {
                PosPrinter = (PosPrinter) posExplorer.CreateInstance(deviceInfo);
            }
            catch (Exception)
            {
                throw new TicketPrinterException($"Impossible de créer une instance de l'imprimante {PrinterName}.");
            }

            try
            {
                //Open the device
                PosPrinter.Open();
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException($"Impossible d'ouvrir l'imprimante {PrinterName}.");
            }

            try
            {
                //Get the exclusive control right for the opened device.
                //Then the device is disable from other application.
                PosPrinter.Claim(1000);
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException($"Impossible de créer une instance de l'imprimante {PrinterName}.");
            }

            try
            {
                //Enable the device.
                PosPrinter.DeviceEnabled = true;
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException("Disable to use the device.");
            }

            try
            {
                //<<<step3>>>--Start
                //Output by the high quality mode
                PosPrinter.RecLetterQuality = true;

                // Even if using any printers, 0.01mm unit makes it possible to print neatly.
                PosPrinter.MapMode = MapMode.Metric;
            }
            catch (PosControlException)
            {
            }
        }

        public void SetUpImages(List<ITicket> tickets)
        {
            var i = 1;
            foreach (var ticket in tickets)
                ticket.SetImage(PosPrinter, i++);
        }


        public void Print(List<ITicket> tickets)
        {
            /*
             *  Initialize printer
             */

            try
            {
                PosPrinter.TransactionPrint(PrinterStation.Receipt,
                    PrinterTransactionControl.Transaction);
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException("Impossible de créer une nouvelle transaction.");
            }

            /*
             *  Write data to printer.
             */

            try
            {
                foreach (var ticket in tickets)
                {
                    ticket.Print(PosPrinter);
                    if (!PosPrinter.CapRecPaperCut) continue;
                    PosPrinter.CutPaper(99);
                }
            }
            catch (PosControlException)
            {
                throw new TicketPrinterException("Impossible d'envoyer les données à l'imprimante.");
            }

            /*
             * Wait that the printer isnt in idle anymore.
             */

            while (PosPrinter.State != ControlState.Idle)
            {
                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            /*
             *  Finalize the print
             */

            PosPrinter.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
        }

        public void Close()
        {
            try
            {
                PosPrinter.DeviceEnabled = false;

                //Release the device exclusive control right.
                PosPrinter.Release();
            }
            catch (PosControlException)
            {
            }
            finally
            {
                PosPrinter.Close();
            }
        }
    }
}