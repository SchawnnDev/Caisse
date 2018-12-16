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
using CaisseDesktop.Graphics.Admin.Checkouts.Pages;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Owners
{
    /// <summary>
    /// Interaction logic for OwnerManager.xaml
    /// </summary>
    public partial class OwnerManager
    {
        public EvenementManager EventManager { get; set; }
        public SaveableOwner SaveableOwner { get; set; }
        private bool Saved { get; set; } = false;
        private bool New { get; set; } = true;

        public OwnerManager(EvenementManager eventManager, SaveableOwner owner)
        {
            InitializeComponent();
            EventManager = eventManager;
            SaveableOwner = owner;
            New = owner == null;
            Saved = !New;
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Blocage_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void DeletePermission_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}