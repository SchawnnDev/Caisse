using System.Windows;

namespace CaisseDesktop.Utils
{
    public class Validations
    {

	    public static bool Ask(string message)
	    {
		    return MessageBox.Show(message, "Validation demandée", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
		}

		public static bool ShowErrorRequest(string error)
	    {
		    return MessageBox.Show(error, "Une erreur est survenue", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes;
	    }

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