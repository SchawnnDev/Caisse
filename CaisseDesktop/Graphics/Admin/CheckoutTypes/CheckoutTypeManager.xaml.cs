using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Media;
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
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Admin.CheckoutTypes
{
    /// <summary>
    /// Interaction logic for CheckoutTypeManager.xaml
    /// </summary>
    public partial class CheckoutTypeManager
    {
        public SaveableCheckoutType CheckoutType { get; set; }
        public EvenementManager Manager { get; }
        private ArticleModel Model => DataContext as ArticleModel;
        private bool New { get; set; }

        public CheckoutTypeManager(EvenementManager manager, SaveableCheckoutType type)
        {
            InitializeComponent();

            CheckoutType = type;
            Manager = manager;
            New = type == null;

            Task.Run(() => Load());

            if (New) return;
            
            CheckoutTypeName.Text = type.Name;

            Task.Run(() => LoadCheckoutNames());

        }

        public void Add(SaveableArticle article)
        {
            Model.Articles.Add(article);
        }

        public void Update()
        {
            ArticlesGrid.Items.Refresh();
        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = new ArticleModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            ObservableCollection<SaveableArticle> collection;

            using (var db = new CaisseServerContext())
            {
                collection =
                    New
                        ? new ObservableCollection<SaveableArticle>()
                        : new ObservableCollection<SaveableArticle>(db.Articles.Where(t => t.Type.Id == CheckoutType.Id)
                            .OrderBy(t => t.Position).ToList());
            }

            Dispatcher.Invoke(() =>
            {
                Model.Articles = collection;
                Mouse.OverrideCursor = null;
            });
        }


        private async void LoadCheckoutNames()
        {
            if (CheckoutType == null) return;

            using (var db = new CaisseServerContext())
            {
                var checkoutNames = await db.Checkouts.Where(t => t.CheckoutType.Id == CheckoutType.Id)
                    .Select(t => t.Name).ToListAsync();
                CheckoutNameList.Dispatcher.Invoke(() =>
                {
                    foreach (var checkoutName in checkoutNames)
                        CheckoutNameList.Items.Add(checkoutName);
                });
            }
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableArticle article)
                new ArticleManager(this, article).ShowDialog();
            else
                MessageBox.Show($"{btn} : le type de caisse n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddArticle_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckoutType == null)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Veuillez d'abord enregistrer les informations obligatoires.", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            new ArticleManager(this, null).ShowDialog();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (CustomPage.Check(CheckoutTypeName))
                return;

            if (New)
            {
                CheckoutType = new SaveableCheckoutType
                {
                    Event = Manager.Evenement
                };
            }

            CheckoutType.Name = CheckoutTypeName.Text;

            Task.Run(() => Save());
        }


        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                if (New)
                {
                    db.Events.Attach(CheckoutType.Event);
                    db.CheckoutTypes.Add(CheckoutType);
                }
                else
                {
                    db.CheckoutTypes.Attach(CheckoutType);
                }


                db.Entry(CheckoutType).State = New ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                if (Manager.MasterFrame.ToCustomPage().CustomName.Equals("EventCheckoutTypePage"))
                {
                    if (New) Manager.MasterFrame.ToCustomPage().Add(CheckoutType);
                    else Manager.MasterFrame.ToCustomPage().Update();
                }

                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "Le type de caisse a bien été crée !" : "Le type de caisse a bien été enregistré !");
            });
        }
    }
}