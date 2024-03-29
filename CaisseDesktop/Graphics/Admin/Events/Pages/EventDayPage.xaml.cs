﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using CaisseDesktop.Graphics.Admin.Days;
using CaisseDesktop.Graphics.Admin.Owners;
using CaisseDesktop.Models;
using CaisseDesktop.Models.Admin;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Events.Pages
{
    /// <summary>
    /// Interaction logic for EventDayPage.xaml
    /// </summary>
    public partial class EventDayPage
    {
        private EventManagerModel ParentModel { get; }

        public EventDayPage(EventManagerModel parentModel)
        {
            InitializeComponent();
            ParentModel = parentModel;

            New = ParentModel.SaveableEvent == null;

            Task.Run(Load);
        }

        private JourModel JourModel => DaysGrid.DataContext as JourModel;
        private bool New { get; }

        public override string CustomName => "EventDayPage";

        public override void Add<T>(T t)
        {
            JourModel.Jours.Add(t as SaveableDay);
        }

        public override void Update()
        {
            DaysGrid.Items.Refresh();
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                DaysGrid.DataContext = new JourModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            var daysCollection = new ObservableCollection<SaveableDay>();

            if (!New)
                using (var db = new CaisseServerContext())
                {
                    daysCollection = new ObservableCollection<SaveableDay>(db.Days
                        .Where(t => t.Event.Id == ParentModel.SaveableEvent.Id).Include(t => t.Event)
                        .OrderByDescending(t => t.Start)
                        .ToList());
                }

            Dispatcher.Invoke(() =>
            {
                JourModel.Jours = daysCollection;
                Mouse.OverrideCursor = null;
            });
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableDay day)
            {
	            new DayManager(ParentModel, day).ShowDialog();
            }
            else
                MessageBox.Show($"{btn} : le jour n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableDay day)
            {
                var result = MessageBox.Show("Es tu sûr de vouloir supprimer ce jour ?",
                    "Supprimer un jour",
                    MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result != MessageBoxResult.Yes) return;

                using (var db = new CaisseServerContext())
                {
                    db.Days.Attach(day);
                    db.Days.Remove(day);
                    db.SaveChanges();
                }

                JourModel.Jours.Remove(day);
            }
            else
            {
                MessageBox.Show($"{btn} : le jour n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public override bool CanClose() => true;

        public override bool CanBack() => true;
    }
}