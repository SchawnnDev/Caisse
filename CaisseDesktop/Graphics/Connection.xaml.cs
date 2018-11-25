using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

namespace CaisseDesktop.Graphics
{
    /// <summary>
    /// Interaction logic for Connection.xaml
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
                CashierId.Text = CashierId.Text += number;
            } else if (str.EndsWith("Supprimer"))
            {
                var len = CashierId.Text.Length;

                if (len != 0)
                {
                    CashierId.Text = CashierId.Text.Remove(len - 1);
                }

            } else if (str.EndsWith("Valider"))
            {

            }
        }


    }
}
