using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseServer;
using CaisseServer.Events;
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
        private MaxSellNumberModel Model => DataContext as MaxSellNumberModel;

        public ArticleManager(SaveableArticle article)
        {
            InitializeComponent();

            Article = article;
            New = article == null;

            if (New)
                Article = new SaveableArticle();
            else Fill();

            Task.Run(() => Load());

            Loaded += (sender, args) =>
            {
                Start = false;
                SwitchButtons(GetIndex(Article.ItemType));
            };


        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = new MaxSellNumberModel();
                Mouse.OverrideCursor = Cursors.Wait;
            });

            ObservableCollection<SaveableArticleMaxSellNumber> collection;
            List<SaveableDay> days;

            using (var db = new CaisseServerContext())
            {
                collection = New ? new ObservableCollection<SaveableArticleMaxSellNumber>() :new ObservableCollection<SaveableArticleMaxSellNumber>(db.ArticleMaxSellNumbers.Where(e => e.Article.Id == Article.Id).Include(e => e.Day).OrderByDescending(e => e.Day.Start).ToList());

                days = New ? new List<SaveableDay>() : db.Days.Where(t => t.Event.Id == Article.Type.Event.Id).OrderBy(t => t.Start).ToList();

                foreach (var day in days)
                {
                    var date = day.Start;
                    if (!collection.Any(t => t.Day.Start.Year == date.Year && t.Day.Start.Month == date.Month && t.Day.Start.Day == date.Day)) continue;
                    ArticleDaysBox.Items.Add(new ComboBoxItem
                    {
                        Content = date.ToString("dd/MM/yyyy"),
                        DataContext = day
                    });
                }

            }

            Dispatcher.Invoke(() =>
            {

                Model.MaxSellNumbers = collection;

                if (days.Count == 0)
                {
                    MaxSellPerDayBox.IsEnabled = false;
                    ArticleDaysBox.IsEnabled = false;
                    //AddMaxSellPerDayButton.IsEnabled = false;
                    AddMaxSellPerDayButton.Content = "Infos";
                }
                
                Mouse.OverrideCursor = null;
            });
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

        public void SwitchButtons(int type)
        {

            switch (type)
            {
                case 0:
                    ArticleNeedsCup.IsEnabled = false;
                    ArticleNeedsCup.IsChecked = false;
                    Article.NeedsCup = false;
                    ArticleTracking.IsEnabled = true;
                    break;
                case 1:
                    ArticleNeedsCup.IsEnabled = true;
                    ArticleTracking.IsChecked = false;
                    Article.NumberingTracking = false;
                    ArticleTracking.IsEnabled = false;
                    break;
                case 2:
                    ArticleNeedsCup.IsChecked = false;
                    ArticleTracking.IsChecked = false;
                    Article.NumberingTracking = false;
                    Article.NeedsCup = false;
                    ArticleNeedsCup.IsEnabled = false;
                    ArticleTracking.IsEnabled = false;
                    break;
            }

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

            if (Article == null || string.IsNullOrWhiteSpace(Article.ImageSrc)) { 
                EditImage(id); // change image when select other type
                SwitchButtons(id);
            }
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

            if (AddMaxSellPerDayButton.Content.Equals("Infos"))
            {
                MessageBox.Show("Veuillez ajouter des jours dans la page de gestion de l'évenement.");
                return;
            }

            if (CustomPage.Check(MaxSellPerDayBox)) return;


        }

    }
}
