using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Admin.Events;
using CaisseServer;

namespace CaisseDesktop.Graphics.Admin.CheckoutTypes
{
    /// <summary>
    /// Interaction logic for CheckoutTypeManager.xaml
    /// </summary>
    public partial class CheckoutTypeManager
    {
        public SaveableCheckoutType CheckoutType { get; set; }
        public EvenementManager Manager { get; }

        public CheckoutTypeManager(EvenementManager manager, SaveableCheckoutType type)
        {
            InitializeComponent();

            CheckoutType = type;
            Manager = manager;

            if (type == null) return;

            Task.Run(() => LoadCheckoutNames());
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
            throw new NotImplementedException();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddArticle_OnClick(object sender, RoutedEventArgs e)
        {
            new ArticleManager(this,null).ShowDialog();
        }
    }
}