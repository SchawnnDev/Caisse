using System.Windows;

namespace CaisseDesktop.Utils
{
    public class Validations
    {
        public static void ShowError(string error)
        {
            MessageBox.Show(error, "Une erreur est survenue", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowWarning(string warning)
        {
            MessageBox.Show(warning, "Attention!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public static bool WillClose(bool page)
        {
            return MessageBox.Show($"Es-tu sûr de vouloir quitter {(page ? "la page" : "l'application")} ?",
                       $"Quitter {(page ? "la page" : "l'application")}", MessageBoxButton.YesNo) ==
                   MessageBoxResult.Yes;
        }
    }
}