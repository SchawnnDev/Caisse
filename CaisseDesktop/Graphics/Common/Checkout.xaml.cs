using System;
using System.Collections;
using System.Collections.Generic;
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
using CaisseServer;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout
    {
        public Checkout()
        {
            InitializeComponent();
            CreateItemGrid(new List<SaveableItem>
            {
                new SaveableItem
                {
                    Name = "Bananes",
                    Price = 1.50M,
                    ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                }, new SaveableItem
                {
                    Name = "Cacao",
                    Price = 1.50M,
                    ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                }, new SaveableItem
                {
                    Name = "Café",
                    Price = 1.50M,
                    ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                }, new SaveableItem
                {
                    Name = "Chocolat",
                    Price = 1M,
                    ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                }, new SaveableItem
                {
                    Name = "Pommes",
                    Price = 1.8M,
                    ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                },
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateItemGrid(List<SaveableItem> items)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            foreach (var item in items)
                panel.Children.Add(CreateIten(item));

            MainGrid.Children.Add(panel);

        }

        private Border CreateIten(SaveableItem item)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(1.0),
                BorderBrush = Brushes.DimGray,
                Margin = new Thickness(50.0)
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

            var img = new Image
            {
                MaxWidth = 300.0,
                MaxHeight = 55.0
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
                Content = "-"
            };

            var textBox = new TextBox
            {
                Text = "0"
                ,
                Padding = new Thickness(1.0),
                MinWidth = 30.0,
                Foreground = Brushes.White,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1.0)
            };

            var plusBtn = new Button
            {
                Content = "+",
            };

            plusBtn.Click += (sender, args) =>
            {
                textBox.Text = int.TryParse(textBox.Text, out var actualValue) ? (actualValue+1).ToString() : "0";
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
            panel.Children.Add(img);
            panel.Children.Add(stackPanel);
            panel.Children.Add(remainingPanel);

            border.Child = panel;

            return border;
        }
    }
}
