using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaisseServer;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    ///     Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout
    {
        public Checkout()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
                CreateItemGrid(new List<SaveableItem>
                {
                    new SaveableItem
                    {
                        Name = "Bananes",
                        Price = 1.50M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableItem
                    {
                        Name = "Cacao",
                        Price = 1.50M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableItem
                    {
                        Name = "Café",
                        Price = 1.50M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableItem
                    {
                        Name = "Chocolat chaud",
                        Price = 1M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableItem
                    {
                        Name = "Pommes",
                        Price = 1.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableItem
                    {
                        Name = "Vin",
                        Price = 100.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableItem
                    {
                        Name = "Patate",
                        Price = 1.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableItem
                    {
                        Name = "Poms",
                        Price = 1.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    }
                });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CreateItemGrid(List<SaveableItem> items)
        {
            var panel = new WrapPanel
            {
                Orientation = Orientation.Horizontal
                //MaxWidth = ActualWidth
            };

            foreach (var item in items)
                panel.Children.Add(CreateItem(item));

            MainGrid.Children.Add(panel);
        }

        private Border CreateItem(SaveableItem item)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(1.0),
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
                Foreground = Brushes.White
            };

            var priceLabel = new Label
            {
                FontSize = 10.0,
                Content = $"{item.Price} €",
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White
            };

            var img = new Image
            {
                MaxWidth = 400.0,
                MaxHeight = 65.0
            };

            var icon = new BitmapImage();
            icon.BeginInit();
            icon.UriSource = new Uri(item.ImageSrc);
            icon.EndInit();

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
            };

            var textBox = new TextBox
            {
                Text = "0",
                Padding = new Thickness(1.0),
                MinWidth = 30.0,
                Foreground = Brushes.White,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(2.0),
            };

            var plusBtn = new Button
            {
                Content = "+",
            };

            minusBtn.Click += (sender, args) =>
            {
                textBox.Text = Math.Max(0, int.TryParse(textBox.Text, out var actualValue) ? actualValue - 1 : 0)
                    .ToString();
            };

            plusBtn.Click += (sender, args) =>
            {
                textBox.Text = int.TryParse(textBox.Text, out var actualValue) ? (actualValue + 1).ToString() : "0";
            };
            stackPanel.Children.Add(minusBtn);
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(plusBtn);

            var remainingPanel = new Label
            {
                Content = "100 restants",
                Foreground = Brushes.White,
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
    }
}