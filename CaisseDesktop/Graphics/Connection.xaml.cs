using System.Media;
using System.Windows;
using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin;

namespace CaisseDesktop.Graphics
{
    /// <summary>
    ///     Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : Window
    {
        public Connection()
        {
            InitializeComponent();
        }

        private void PinPadButton_Click(object sender, RoutedEventArgs e)
        {
            var buttonContent = (sender as Button)?.Content;

            if (buttonContent == null) return;

            var str = buttonContent.ToString();

            if (int.TryParse(str, out var number))
            {
                if (CashierId.Text.Length < 7)
                    CashierId.Text = CashierId.Text += number;
                else
                    SystemSounds.Beep.Play();
            }
            else if (str.EndsWith("Supprimer"))
            {
                var len = CashierId.Text.Length;

                if (len != 0)
                    CashierId.Text = CashierId.Text.Remove(len - 1);
                else
                    SystemSounds.Beep.Play();
            }
            else if (str.EndsWith("Valider"))
            {
                if (CashierId.Text.Length != 7)
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Le n° n'est pas valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //TODO: Si ce n'est pas encore l'heure du caissier, le prevenir et demander si il est sûr de vouloir continuer
                    MessageBox.Show("Bien connecté.", "Yeah", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private void Admin_OnClick(object sender, RoutedEventArgs e)
        {
            new AdminConnection().Show();
            Close();
        }
    }
}