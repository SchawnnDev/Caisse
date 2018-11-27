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
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events
{
    /// <summary>
    /// Interaction logic for EvenementManager.xaml
    /// </summary>
    public partial class EvenementManager : Window
    {
        public SaveableEvent Evenement;


        public EvenementManager(SaveableEvent evenement)
        {
            InitializeComponent();
            Evenement = evenement;

            if (evenement != null)
                FillTextBoxes();
        }

        private void FillTextBoxes()
        {
            EventName.Text = Evenement.Name;
            EventStart.Text = Evenement.Start.ToLongDateString();
            EventEnd.Text = Evenement.End.ToLongDateString();
            EventDescription.Text = Evenement.Description;
            EventAddresse.Text = Evenement.Addresse;

        }
    }
}