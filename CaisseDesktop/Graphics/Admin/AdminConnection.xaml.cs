using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace CaisseDesktop.Graphics.Admin
{
    /// <summary>
    ///     Interaction logic for AdminConnection.xaml
    /// </summary>
    public partial class AdminConnection : Window
    {
        public AdminConnection()
        {
            InitializeComponent();
            Buttons = new[] {One, Two, Three, Four, Five, Six, Seven, Eight, Nine};
        }

        private bool DisplayingNumbers { get; set; } = true;
        private Button[] Buttons { get; }
        private string Password { get; set; } = "";

        private void PinPadButton_Click(object sender, RoutedEventArgs e)
        {
            var buttonContent = (sender as Button)?.Content;

            if (buttonContent == null) return;

            var str = buttonContent.ToString();

            if (str.Length == 1)
            {
                if (CashierId.Text.Length < 7)
                {
                    if (int.TryParse(str, out var number))
                    {
                        CashierId.Text = CashierId.Text += "•";
                        Password = Password += number;
                    }
                    else
                    {
                        CashierId.Text = CashierId.Text += "•";
                        Password = Password += str;
                    }
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
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


        private void Retour_OnClick(object sender, RoutedEventArgs e)
        {
            //new Connection().Show();
            Close();
        }

        private void Switch_OnClick(object sender, RoutedEventArgs e)
        {
            var pattern = "ABCXYZ/*-";
            var count = 0;

            if (DisplayingNumbers)
            {
                foreach (var button in Buttons)
                    button.Content = pattern[count++];

                DisplayingNumbers = false;
                return;
            }

            foreach (var button in Buttons)
                button.Content = count++ + 1;

            DisplayingNumbers = true;
        }
    }
}