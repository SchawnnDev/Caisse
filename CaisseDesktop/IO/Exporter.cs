using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CaisseDesktop.IO
{
    public class Exporter
    {
        public static string AskDestinationPath()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Select a folder",
                CheckPathExists = true,
                Filter = "CSV Files|*.csv"
            };

            if (saveFileDialog.ShowDialog() != true) return null;

            var path = saveFileDialog.FileName;


            return null;
        }

        private static string[] ReadFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}