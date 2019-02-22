using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaisseDesktop.Utils;
using CaisseServer.Items;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace CaisseDesktop.Graphics.Admin.Articles
{
    /// <summary>
    /// Interaction logic for ArticleManager.xaml
    /// </summary>
    public partial class ArticleManager
    {
        private static readonly Regex PasteRegex = new Regex("([0-9]*[.])?[0-9]+"); //regex that matches allowed text
        private static readonly Regex InsertRegex = new Regex("([0-9]|[.]|[,])"); //regex that matches allowed text
        private static readonly Regex OnlyNumbersRegex = new Regex("([0-9])"); //regex that matches allowed text
        private SaveableArticle Article { get; }
        private bool Start { get; set; } = true;
         
        private bool New { get; set; }
        public ArticleManager(SaveableArticle article)
        {
            InitializeComponent();

            Article = article;
            New = article == null;

            if (New)
                Article = new SaveableArticle();
            else Fill();

            Loaded += (sender, args) => { Start = false; };


        }

        public void Fill()
        {
            ArticleName.Text = Article.Name;
            ArticleType.SelectedIndex = GetIndex(Article.ItemType);
            ArticlePrice.Text = Article.Price.ToString(CultureInfo.CurrentCulture);
            ArticleMaxSellPerDay.Text = Article.MaxSellNumberPerDay.ToString();
            ArticleActivated.IsChecked = Article.Active;
            ArticleImage.Source = new BitmapImage(new Uri(Article.ImageSrc));
            ArticleImagePath.Text = Article.ImageSrc;
            ArticleColor.SelectedColor = System.Drawing.ColorTranslator.FromHtml(Article.Color).Convert();
            ArticleNeedsCup.IsChecked = Article.NeedsCup;
            ArticleTracking.IsChecked = Article.NumberingTracking;
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

            var id = GetIndex((string)item.Content);

            if (Article == null || string.IsNullOrWhiteSpace(Article.ImageSrc))
                EditImage(id); // change image when select other type
        }

        private int GetIndex(string name)
        {
            switch (name)
            {
                /*case "Tickets":
                    return 0; */
                case "Alimentation":
                    return 1;
                case "Consignes":
                    return 2;
                default:
                    return 0;
            }
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
            ArticleImagePath.Text = $"../Resources/Images/{name}.png";

        }

        private void EditImageFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Selectionne une image",
                InitialDirectory = New || string.IsNullOrWhiteSpace(Article.ImageSrc) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : Article.ImageSrc,
                Filter = "Fichier images|*.png;*.jpg;*.jpeg;*.gif"
            };

            if (openFileDialog.ShowDialog() != true) return;

            var path = openFileDialog.FileName;
            ArticleImagePath.Text = path;
            ArticleImage.Source = new BitmapImage(new Uri(path));

        }

        private void Active_OnClick(object sender, RoutedEventArgs e)
        {
            Article.Active = !Article.Active;
        }

        private void NeedsCup_OnClick(object sender, RoutedEventArgs e)
        {
            Article.NeedsCup = !Article.NeedsCup;
        }

        private void DeleteMaxSellNumber_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void OnlyNumbers_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (text != null && !OnlyNumbersRegex.IsMatch(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void AddMaxSellPerDay_OnClick(object sender, RoutedEventArgs e)
        {

            if (CustomPage.Check(MaxSellPerDayBox)) return;


        }

    }
}
