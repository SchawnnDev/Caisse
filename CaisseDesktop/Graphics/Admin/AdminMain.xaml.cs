using System;
using System.Collections.Generic;
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
using CaisseDesktop.Graphics.Admin.Events;

namespace CaisseDesktop.Graphics.Admin
{
    /// <summary>
    /// Interaction logic for AdminMain.xaml
    /// </summary>
    public partial class AdminMain : Window
    {
        public AdminMain()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var browser = new EvenementBrowser();
            browser.ShowDialog();
        }
    }
}
