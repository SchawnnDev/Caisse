﻿using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CaisseLibrary;
using CaisseServer;

namespace CaisseDesktop
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) => Task.Run(() => Start());
        }

        public void Start()
        {
            using (var db = new CaisseServerContext())
            {
                //TEST
                //db.Database.Delete();
                StatusText.Dispatcher.Invoke(() => SetStatusText(0));
                db.Database.CreateIfNotExists();
              //  Database.SetInitializer(new MigrateDatabaseToLatestVersion<CaisseServerContext, EF6Console.Migrations.Configuration>());

            }

            StatusText.Dispatcher.Invoke(() => SetStatusText(1));

            Main.Start();

            // Start the application

            StatusText.Dispatcher.Invoke(() => SetStatusText(2));

            Thread.Sleep(2000);

            Dispatcher.Invoke(() =>
            {
                new SelectionWindow().Show();
                Close();
            });
        }

        private void SetStatusText(int id)
        {
            switch (id)
            {
                case 0:
                    StatusText.Text = "Creation de la base de données...";
                    break;
                case 1:
                    StatusText.Text = "Chargement de la librairie...";
                    break;
                case 2:
                    StatusText.Text = "Démarrage de l'application...";
                    break;
            }
        }
    }
}