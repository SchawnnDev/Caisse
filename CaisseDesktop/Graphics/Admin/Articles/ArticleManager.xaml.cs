using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Admin.Articles
{
    /// <summary>
    /// Interaction logic for ArticleManager.xaml
    /// </summary>
    public partial class ArticleManager : Window
    {
        private static readonly Regex PasteRegex = new Regex("([0-9]*[.])?[0-9]+"); //regex that matches allowed text
        private static readonly Regex InsertRegex = new Regex("([0-9]|[.]|[,])"); //regex that matches allowed text

        public ArticleManager(SaveableArticle item)
        {
            InitializeComponent();
        }

        private void Price_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !InsertRegex.IsMatch(e.Text);

        }

        private void Number_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (text != null && !PasteRegex.IsMatch(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
    }

    }
}
