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

namespace CaisseDesktop.Graphics.Admin.TimeSlots
{
    /// <summary>
    /// Interaction logic for TimeSlotManager.xaml
    /// </summary>
    public partial class TimeSlotManager : Window
    {
        public TimeSlotManager()
        {
            InitializeComponent();
        }

        private void TimeSlotCashier_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void TimeSlotStart_OnSelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
        }

        private void TimeSlotEnd_OnSelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
        }

        private void TimeSlotPause_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
