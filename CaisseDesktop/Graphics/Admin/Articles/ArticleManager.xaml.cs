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
using CaisseDesktop.Models.Admin.Articles;
using CaisseDesktop.Models.Admin.CheckoutTypes;
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
        private static readonly Regex OnlyNumbersRegex = new Regex("([0-9])"); //regex that matches allowed text
        private SaveableArticle Article { get; }
        private bool New { get; set; }
        private ArticleConfigModel Model => DataContext as ArticleConfigModel;
        private readonly CheckoutTypePage ParentModel;

        public ArticleManager(CheckoutTypePage model, SaveableArticle article)
        {
            InitializeComponent();

            Article = article;
            New = article == null;
            ParentModel = model;
            DataContext = new ArticleConfigModel(article, model);
            Model.Dispatcher = Dispatcher;
            Model.CloseAction = Close;

            Task.Run(Load);

        }

        private void Load()
        {
            Dispatcher.Invoke(() =>
            {
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

                days = db.Days.Where(t => t.Event.Id == ParentModel.CheckoutType.Event.Id).OrderBy(t => t.Start).ToList();
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

    }
}