using System;
using System.Windows;
using CaisseDesktop.Utils;
using Microsoft.PointOfService;

namespace CaisseDesktop.Graphics.Print
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
                Validations.ShowError($"Impossible d'établir une connexion avec l'imprimante {PrinterName}.");
                return;
            }

            try
            {
                PosPrinter = (PosPrinter) posExplorer.CreateInstance(deviceInfo);
            }
            catch (Exception)
            {
                Validations.ShowError($"Impossible de créer une instance de l'imprimante {PrinterName}.");
                return;
            }

            try
            {
                //Open the device
                PosPrinter.Open();
            }
            catch (PosControlException)
            {
                Validations.ShowError($"Impossible d'ouvrir l'imprimante {PrinterName}.");
                return;
            }

            try
            {
                //Get the exclusive control right for the opened device.
                //Then the device is disable from other application.
                PosPrinter.Claim(1000);
            }
            catch (PosControlException)
            {
                Validations.ShowError($"Impossible de créer une instance de l'imprimante {PrinterName}.");
                return;
            }

            try
            {
                //Enable the device.
                PosPrinter.DeviceEnabled = true;
            }
            catch (PosControlException)
            {
                MessageBox.Show("Disable to use the device.", "Printer_SampleStep13");
                return;
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

    }
}