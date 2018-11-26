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
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    /// Interaction logic for EvenementMain.xaml
    /// </summary>
    public partial class EvenementMain : Window
    {
        public List<SaveableEvent> Events { get; }

        public EvenementMain()
        {
            InitializeComponent();

            using (var db = new CaisseServerContext())
                Events = db.Events.OrderBy(e => e.End).ToList();
        }
    }
}