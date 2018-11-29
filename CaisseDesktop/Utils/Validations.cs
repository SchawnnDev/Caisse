using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CaisseDesktop.Utils
{
    public class Validations
    {
        public static bool WillClose(bool page) =>
            MessageBox.Show($"Es-tu sûr de vouloir quitter {(page ? "la page" : "l'application")} ?",
                $"Quitter {(page ? "la page" : "l'application")}", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
    }
}