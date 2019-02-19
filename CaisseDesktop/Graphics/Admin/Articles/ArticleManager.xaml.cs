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
using Microsoft.Win32;
using Path = System.IO.Path;

namespace CaisseDesktop.Graphics.Admin.Articles
{
    /// <summary>
    /// Interaction logic for ArticleManager.xaml
    /// </summary>
    public partial class ArticleManager : Window
    {
        private static readonly Regex PasteRegex = new Regex("([0-9]*[.])?[0-9]+"); //regex that matches allowed text
        private static readonly Regex InsertRegex = new Regex("([0-9]|[.]|[,])"); //regex that matches allowed text
        private SaveableArticle Article { get; }
        private bool Start { get; set; } = true;

        private bool New { get; set; }
        public ArticleManager(SaveableArticle article)
        {
            InitializeComponent();

            Article = article;
            New = article == null;

            Loaded += (sender, args) => { Start = false; };


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

        private void Type_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Start || e.AddedItems.Count != 1 || !(e.AddedItems[0] is ComboBoxItem item)) return;

            var id = 0;

            switch (item.Content)
            {
                case "Tickets":
                    break;
                case "Alimentation":
                    id = 1;
                    break;
                case "Consignes":
                    id = 2;
                    break;
                default:
                    return;
            }

            if (Article == null || string.IsNullOrWhiteSpace(Article.ImageSrc))
                EditImage(id); // change image when select other type
        }

        private void EditImage(int type)
        {

            var name = "ticket";

            switch (type)
            {
                case 0:
                    break;
                case 1:
                    name = "food";
                    break;
                case 2:
                    name = "cup";
                    break;
                default:
                    return;
            }


            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Resources/Images/{name}.png");

            ArticleImage.Source = new BitmapImage(new Uri(path));
            ImagePath.Text = $"../Resources/Images/{name}.png";

        }

        private void EditImageFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Selectionne une image",
                InitialDirectory = New || string.IsNullOrWhiteSpace(Article.ImageSrc) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : Article.ImageSrc,
                Filter = "Fichier images|*.png;*.jpg;*.jpeg;*.gif"
            };

            if (openFileDialog.ShowDialog() != true) return;

            var path = openFileDialog.FileName;
            ImagePath.Text = path;
            ArticleImage.Source = new BitmapImage(new Uri(path));

        }
    }
}
