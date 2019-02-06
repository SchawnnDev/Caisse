using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseLibrary.IO;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {
        public Login()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                if (!ConfigFile.GetConfig().ContainsKey("event_id"))
                {
                    new Parameters().ShowDialog();
                }

            };


        }


        private void OpenParameters_OnClick(object sender, RoutedEventArgs e)
        {
            new Parameters().ShowDialog();
        }

        private void PinPadButton_Click(object sender, RoutedEventArgs e)
        {
            var buttonContent = (sender as Button)?.Content;

            if (buttonContent == null) return;

            var str = buttonContent.ToString();

            if (int.TryParse(str, out var number))
            {
                if (Password.Password.Length < 7)
                    Password.Password = Password.Password += number;
                else
                    SystemSounds.Beep.Play();
            }
            else if (str.EndsWith("Suppr."))
            {
                var len = Password.Password.Length;

                if (len != 0)
                    Password.Password = Password.Password.Remove(len - 1);
                else
                    SystemSounds.Beep.Play();
            }
            else if (str.EndsWith("Annuler"))
            {

                Password.Password = "";
           
            }
        }

        private void Valider_OnClick(object sender, RoutedEventArgs e)
        {
            if (Password.Password.Length != 7)
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
}
