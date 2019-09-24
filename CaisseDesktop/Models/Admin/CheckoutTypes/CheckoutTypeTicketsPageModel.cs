using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CaisseDesktop.Graphics.Admin.Articles;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.IO;
using CaisseLibrary.IO;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Models.Admin.CheckoutTypes
{
    public class CheckoutTypeTicketsPageModel : CheckoutTypePage
    {
        public readonly CheckoutTypeConfigModel ParentModel;

        public CheckoutTypeTicketsPageModel(CheckoutTypeConfigModel parentModel) : base(parentModel.CheckoutType)
        {
            ParentModel = parentModel;
            Task.Run(LoadArticles);
        }

        public override void LoadArticles()
        {
            if (ParentModel.IsCreating)
            {
                Articles = new ObservableCollection<CheckoutTypeArticle>();
                return;
            }

            ParentModel.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            ObservableCollection<CheckoutTypeArticle> list;

            using (var db = new CaisseServerContext())
            {
                list = new ObservableCollection<CheckoutTypeArticle>(db.Articles
                    .Where(t => t.Type.Id == ParentModel.CheckoutType.Id).OrderBy(t => t.Position).ToList()
                    .Select(t => new CheckoutTypeArticle(t, this)).ToList());
            }

            ParentModel.Dispatcher.Invoke(() =>
            {
                Articles = list;
                Mouse.OverrideCursor = null;
            });
        }

    }
}