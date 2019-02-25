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
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Days
{
    /// <summary>
    /// Interaction logic for DayManager.xaml
    /// </summary>
    public partial class DayManager : Window
    {

        private EvenementManager Parent { get; }
        private SaveableDay Day { get; }
        private bool New { get; }

        public DayManager(EvenementManager parent, SaveableDay day)
        {
            InitializeComponent();
            this.Owner = parent;
            Parent = parent;
            Day = day;
            New = day == null;

            if (New)
            {
                Day = new SaveableDay();

            }

        }
    }
}
