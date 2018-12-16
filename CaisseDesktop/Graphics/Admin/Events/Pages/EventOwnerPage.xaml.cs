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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    /// Interaction logic for EventOwnerPage.xaml
    /// </summary>
    public partial class EventOwnerPage
    {
        public EventOwnerPage()
        {
            InitializeComponent();
        }

        public override bool CanClose() => true;

        public override bool CanBack() => true;

        public override string CustomName => "EventOwnerPage";

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
        }

    }
}
