using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaisseDesktop.Graphics.Admin.CheckoutTypes;
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
        private CheckoutTypeManager Manager { get; }

        public ArticleManager(CheckoutTypeManager manager, SaveableArticle article)
        {
            InitializeComponent();

            Article = article;
            New = article == null;
            Manager = manager;

            if (New)
            {
                Article = new SaveableArticle
                {
                    Type = manager.CheckoutType
                };
            }
            else
            {
                Fill();
            }

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
                collection = New
                    ? new ObservableCollection<SaveableArticleMaxSellNumber>()
                    : new ObservableCollection<SaveableArticleMaxSellNumber>(db.ArticleMaxSellNumbers
                        .Where(e => e.Article.Id == Article.Id).Include(e => e.Day).OrderByDescending(e => e.Day.Start)
                        .ToList());

                days = db.Days.Where(t => t.Event.Id == Manager.Manager.Evenement.Id).OrderBy(t => t.Start).ToList();
            }

            Dispatcher.Invoke(() =>
            {
                Model.MaxSellNumbers = collection;

                if (days.Count == 0)
                {
                    ToggleMaxSellPerDayBox(false);
                }
                else
                {
                    ArticleDaysBox.Items.Clear();

                    foreach (var day in days)
                    {
                        var date = day.Start;
                        if (collection.Any(t =>
                            t.Day.Start.Year == date.Year && t.Day.Start.Month == date.Month &&
                            t.Day.Start.Day == date.Day)) continue;
                        ArticleDaysBox.Items.Add(new ComboBoxItem
                        {
                            Content = date.ToString("dd/MM/yyyy"),
                            DataContext = day
                        });
                    }

                    ArticleDaysBox.SelectedIndex = 0;
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
            try
            {
                ArticleImage.Source = new BitmapImage(new Uri(Article.ImageSrc));
                ArticleImagePath.Text = Article.ImageSrc;
            }
            catch (Exception e)
            {
                Validations.ShowError(e.Message + " => le fichier a été remplacé par un fichier de base.");
                EditImage(ArticleType.SelectedIndex);
            }

            ArticleColor.SelectedColor = System.Drawing.ColorTranslator.FromHtml(Article.Color).Convert();
            ArticleNeedsCup.IsChecked = Article.NeedsCup;
            ArticleTracking.IsChecked = Article.NumberingTracking;
        }

        public void SwitchButtons(int type)
        {
            switch (type)
            {
                case 0: //tickets
                    ArticleNeedsCup.IsEnabled = false;
                    ArticleNeedsCup.IsChecked = false;
                    Article.NeedsCup = false;
                    ArticleTracking.IsEnabled = true;
                    ArticleMaxSellPerDay.IsEnabled = true;
                    //
                    ToggleMaxSellPerDay(true);
                    break;
                case 1: //alimentation
                    ArticleNeedsCup.IsEnabled = true;
                    ArticleTracking.IsChecked = false;
                    Article.NumberingTracking = false;
                    ArticleTracking.IsEnabled = false;
                    ArticleMaxSellPerDay.IsEnabled = true;
                    //
                    ToggleMaxSellPerDay(true);
                    break;
                case 2: //consignes
                    ArticleMaxSellPerDay.IsEnabled = false;
                    ArticleNeedsCup.IsChecked = false;
                    ArticleTracking.IsChecked = false;
                    Article.NumberingTracking = false;
                    Article.NeedsCup = false;
                    ArticleNeedsCup.IsEnabled = false;
                    ArticleTracking.IsEnabled = false;
                    //
                    ToggleMaxSellPerDay(false);
                    break;
            }
        }

        private void ToggleMaxSellPerDay(bool toggle)
        {
            MaxSellPerDayBox.IsEnabled = toggle && ArticleDaysBox.Items.Count != 0;
            ArticleDaysBox.IsEnabled = toggle && ArticleDaysBox.Items.Count != 0;
            AddMaxSellPerDayButton.IsEnabled = toggle && ArticleDaysBox.Items.Count != 0;
            MaxSellNumber.IsEnabled = toggle;
        }

        private void Price_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !InsertRegex.IsMatch(e.Text);
        }

        private void Number_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string) e.DataObject.GetData(typeof(string));
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

            var id = GetIndex((string) item.Content);

            if (Article == null || string.IsNullOrWhiteSpace(Article.ImageSrc) || EndsWith(Article.ImageSrc))
            {
                EditImage(id); // change image when select other type
                SwitchButtons(id);
            }
        }

        private string GetType(int index)
        {
            var name = "Tickets";
            switch (index)
            {
                case 1:
                    name = "Alimentation";
                    break;
                case 2:
                    name = "Consignes";
                    break;
            }

            return name;
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

        private bool EndsWith(string name)
        {
            var baseString = "Resources\\Images\\";
            return name.EndsWith(baseString + "ticket.jpg") || name.EndsWith(baseString + "food.jpg") || name.EndsWith(baseString + "cup.jpg");
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

            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Resources\\Images\\{name}.jpg");

                ArticleImage.Source = new BitmapImage(new Uri(path));
                ArticleImagePath.Text = $"..\\Resources\\Images\\{name}.jpg";
                Article.ImageSrc = path;
            }
            catch (Exception e)
            {
                Validations.ShowError(e.Message);
            }
        
        }

        private void EditImageFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Selectionne une image",
                InitialDirectory = New || string.IsNullOrWhiteSpace(Article.ImageSrc)
                    ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                    : Article.ImageSrc,
                Filter = "Fichier images|*.jpg;*.jpeg;*.bmp"
            };

            if (openFileDialog.ShowDialog() != true) return;

            var path = openFileDialog.FileName;
            ArticleImagePath.Text = path;
            ArticleImage.Source = new BitmapImage(new Uri(path));
            Article.ImageSrc = path;
        }

        private void Active_OnClick(object sender, RoutedEventArgs e)
        {
            Article.Active = !Article.Active;
        }

        private void NeedsCup_OnClick(object sender, RoutedEventArgs e)
        {
            Article.NeedsCup = !Article.NeedsCup;
        }

        private void Tracking_OnClick(object sender, RoutedEventArgs e)
        {
            Article.NumberingTracking = !Article.NumberingTracking;
        }

        private void DeleteMaxSellNumber_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn?.DataContext is SaveableArticleMaxSellNumber maxSellNumber)
            {
                var result = MessageBox.Show("Es-tu sûr de vouloir supprimer ce nb max de ventes ?",
                    "Supprimer un nb max de ventes",
                    MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result != MessageBoxResult.Yes) return;

                if (maxSellNumber.Article != null)
                {
                    using (var db = new CaisseServerContext())
                    {
                        db.ArticleMaxSellNumbers.Attach(maxSellNumber);
                        db.ArticleMaxSellNumbers.Remove(maxSellNumber);
                        db.SaveChanges();
                    }
                }

                Model.MaxSellNumbers.Remove(maxSellNumber);

                if (ArticleDaysBox.Items.Count == 0)
                    ToggleMaxSellPerDayBox(true);

                ArticleDaysBox.Items.Add(new ComboBoxItem
                {
                    Content = maxSellNumber.Day.Start.ToString("dd/MM/yyyy"),
                    DataContext = maxSellNumber.Day
                });

                ArticleDaysBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show($"{btn} : le nb max de ventes n'est pas valide.", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OnlyNumbers_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string) e.DataObject.GetData(typeof(string));
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
            if (!(ArticleDaysBox.SelectedItem is ComboBoxItem item)) return;
            var day = item.DataContext as SaveableDay;

            var maxSellPerDay = new SaveableArticleMaxSellNumber
            {
                Day = day,
                Amount = int.Parse(MaxSellPerDayBox.Text)
            };

            Model.MaxSellNumbers.Add(maxSellPerDay);
            ArticleDaysBox.Items.RemoveAt(ArticleDaysBox.SelectedIndex);
            ArticleDaysBox.SelectedIndex = 0;
            MaxSellPerDayBox.Text = "";

            if (ArticleDaysBox.Items.Count != 0) return;

            ToggleMaxSellPerDayBox(false);
        }

        public void ToggleMaxSellPerDayBox(bool toggle)
        {
            MaxSellPerDayBox.IsEnabled = toggle;
            ArticleDaysBox.IsEnabled = toggle;
            AddMaxSellPerDayButton.Content = !toggle ? "Infos" : "Ajouter";
        }

        private void OnlyNumbers_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumbersRegex.IsMatch(e.Text);
        }


        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (CustomPage.Check(ArticleName) || CustomPage.Check(ArticlePrice) ||
                CustomPage.Check(ArticleMaxSellPerDay))
                return;

            if (ArticleColor.SelectedColor == null)
            {
                ArticleColor.BorderBrush = Brushes.Red;
                SystemSounds.Beep.Play();
                return;
            }

            Article.Name = ArticleName.Text;
            Article.Price = decimal.Parse(ArticlePrice.Text.Replace('.', ','));
            Article.MaxSellNumberPerDay = int.Parse(ArticleMaxSellPerDay.Text);
            Article.Color = System.Drawing.ColorTranslator.ToHtml(ArticleColor.SelectedColor.Value.Convert());
            Article.ItemType = GetType(ArticleType.SelectedIndex);

            Task.Run(() => Save());
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                if (New)
                {
                    db.CheckoutTypes.Attach(Article.Type);
                    db.Articles.Add(Article);
                }
                else
                {
                    db.Articles.Attach(Article);
                }


                db.Entry(Article).State = New ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(New ? "L'article a bien été crée !" : "L'article a bien été enregistré !");

                if (New)
                    Manager.Add(Article);
                else
                    Manager.Update();

                Close();
            });
        }
    }
}