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
    public partial class Checkout : Window
    {
        public Checkout()
        {
            InitializeComponent();
            CreateItemGrid(new List<SaveableItem>
            {
                new SaveableItem
                {
                    ImageSrc = ""
                }, new SaveableItem
                {

                }, new SaveableItem
                {

                }, new SaveableItem
                {

                }, new SaveableItem
                {

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
            };

            var plusBtn = new Button
            {
                Content = "+"
            };

            stackPanel.Children.Add(minusBtn);
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(plusBtn);

            var remainingPanel = new Label
            {
                Content = "100 restants"
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
