using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaisseDesktop.Utils;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Exceptions;
using CaisseLibrary.Print;
using CaisseServer;
using CaisseServer.Items;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Label = System.Windows.Controls.Label;
using Orientation = System.Windows.Controls.Orientation;
using TextBox = System.Windows.Controls.TextBox;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    ///     Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout
    {
        private SaveableCheckout ActualCheckout { get; set; }
        private List<TextBox> TextBoxes { get; set; }

        public Checkout(SaveableCheckout checkout)
        {
            InitializeComponent();
            TextBoxes = new List<TextBox>();
            ActualCheckout = checkout;
            ConsignTextBox.DataContext = 0;

            Loaded += (sender, args) =>
            {

                if (Main.Articles.Count > 0)
                {
                    CreateItemGrid(Main.Articles);
                }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CreateItemGrid(List<SaveableArticle> items)
        {
            foreach (var item in items)
                ItemPanel.Children.Add(CreateItem(item));
        }

        private string GetDefaultImageName(int type)
        {
            switch (type)
            {
                case 1:
                    return "food";
                case 2:
                    return "cup";
                default:
                    return "ticket";
            }
        }

        private Border CreateItem(SaveableArticle item)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(2.0),
                BorderBrush = Brushes.DimGray,
                Margin = new Thickness(40.0)

                //VerticalAlignment = VerticalAlignment.Top
            };

            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5.0)
            };

            var label = new Label
            {
                FontSize = 20.0,
                Content = item.Name,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.LightSlateGray
            };

            var priceLabel = new Label
            {
                FontSize = 10.0,
                Content = $"{item.Price} €",
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.LightSlateGray
            };

            var img = new Image
            {
                MaxWidth = 400.0,
                MaxHeight = 65.0
            };


            var icon = new BitmapImage();

            try
            {
                icon.BeginInit();
                icon.UriSource = new Uri(item.ImageSrc);
                icon.EndInit();
            }
            catch (Exception)
            {
                icon = new BitmapImage();
                icon.BeginInit();
                icon.UriSource = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $"Resources\\Images\\{GetDefaultImageName(item.ItemType)}.jpg"));
                icon.EndInit();
            }


            img.Source = icon;

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var minusBtn = new Button
            {
                Content = "-",
                IsEnabled = false
            };

            var textBox = new TextBox
            {
                Text = "0",
                DataContext = 0,
                Padding = new Thickness(1.0),
                MinWidth = 30.0,
                Foreground = Brushes.LightSlateGray,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderBrush = Brushes.LightSlateGray,
                BorderThickness = new Thickness(2.0),
            };

            // ajout pour reset
            TextBoxes.Add(textBox);

            var plusBtn = new Button
            {
                Content = "+",
            };

            minusBtn.Click += (sender, args) =>
            {
                var context = (int) textBox.DataContext;

                if (context == 1)
                {
                    textBox.DataContext = 0;
                    textBox.Text = "0";
                    minusBtn.IsEnabled = false;
                    Main.ActualInvoice.SetArticleCount(item, 0);
                } else if (context > 1)
                {
                    textBox.DataContext = context - 1;
                    textBox.Text = $"{context - 1}";
                    Main.ActualInvoice.SetArticleCount(item, context - 1);
                }

                if (item.NeedsCup)
                    UpdateConsign((int)ConsignTextBox.DataContext - 1);

                UpdateLabels();

            };

            plusBtn.Click += (sender, args) =>
            {
                var context = (int)textBox.DataContext;
                textBox.DataContext = context + 1;
                textBox.Text = $"{context + 1}";
                Main.ActualInvoice.SetArticleCount(item,context + 1);

                if (context == 0)
                    minusBtn.IsEnabled = true;

                if (item.NeedsCup)
                    UpdateConsign((int)ConsignTextBox.DataContext + 1);

                UpdateLabels();

            };

            stackPanel.Children.Add(minusBtn);
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(plusBtn);

            var remainingPanel = new Label
            {
                Content = $"{item.MaxSellNumberPerDay} restants",
                Foreground = Brushes.LightSlateGray,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            panel.Children.Add(label);
            panel.Children.Add(priceLabel);
            panel.Children.Add(img);
            panel.Children.Add(stackPanel);
            panel.Children.Add(remainingPanel);

            border.Child = panel;

            return border;
        }

        public void UpdateConsign(int count)
        {
            count = Math.Max(count, 0);

            ConsignTextBox.Text = count.ToString();
            ConsignTextBox.DataContext = count;
            Main.ActualInvoice.Consign.Amount = count;
            ConsignMinus.IsEnabled = count != 0;
        }

        private void SelectPaymentMethod_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Main.ActualInvoice.IsSomething())
            {
                SystemSounds.Beep.Play();
                return;
            }

            new SelectPaymentMethod(this).ShowDialog();
        }

        public void OpenLoading(bool receiptTicket)
        {
            new Loading(this, receiptTicket).ShowDialog();
        }

        public void NewInvoice()
        {
            Main.NewInvoice();

            foreach (var box in TextBoxes)
            {
                box.Text = "0";
                box.DataContext = 0;
            }

            ConsignTextBox.DataContext = 0;
            ConsignTextBox.Text = "0";

            UpdateLabels();
        }

        public void UpdateLabels()
        {
            TotalArticlesLabel.Content = $"Total articles : {Main.ActualInvoice.CalculateTotalArticlesPrice():F} €";
            TotalConsignsLabel.Content = $"Total consignes : {Main.ActualInvoice.Consign.Amount:F} €";
            TotalLabel.Content = $"Total général : {Main.ActualInvoice.CalculateTotalPrice():F} €";
        }

        private void ConsignPlus_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateConsign((int)ConsignTextBox.DataContext + 1);
            UpdateLabels();
        }

        private void ConsignMinus_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateConsign((int)ConsignTextBox.DataContext - 1);
            UpdateLabels();
        }
    }
}