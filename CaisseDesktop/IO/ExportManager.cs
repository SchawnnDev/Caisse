using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace CaisseDesktop.IO
{

    public class ExportManager
    {
        public static void ExportObjectToJSON(string fileName, object obj)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Select a folder",
                CheckPathExists = true,
                Filter = "CSV Files|*.csv",
                FileName = fileName
            };

            if (saveFileDialog.ShowDialog() != true) return;

            File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(obj));

        }
    }

}