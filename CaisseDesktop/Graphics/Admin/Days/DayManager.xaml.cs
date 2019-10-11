using System;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Admin;
using CaisseDesktop.Models.Admin.Days;
using CaisseDesktop.Utils;
using CaisseLibrary.Utils;
using CaisseServer;
using CaisseServer.Events;
using MaterialDesignThemes.Wpf;
using EventManager = CaisseDesktop.Graphics.Admin.Events.EventManager;

namespace CaisseDesktop.Graphics.Admin.Days
{
    /// <summary>
    /// Interaction logic for DayManager.xaml
    /// </summary>
    public partial class DayManager
    {

		public DayConfigModel Model => DataContext as DayConfigModel;

        public DayManager(EventManagerModel model, SaveableDay day)
        {
            InitializeComponent();
            DataContext = new DayConfigModel(model, day);
            Model.Dispatcher = Dispatcher;
            Model.CloseAction = Close;

        }
    }
}