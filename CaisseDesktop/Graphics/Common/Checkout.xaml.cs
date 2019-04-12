using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaisseLibrary;
using CaisseLibrary.Concrete.Invoices;
using CaisseLibrary.Print;
using CaisseServer;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Common
{
    /// <summary>
    ///     Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout
    {
        public Invoice TempInvoice { get; set; }
        private SaveableCheckout ActualCheckout { get; set; }

        public Checkout(SaveableCheckout checkout)
        {
            InitializeComponent();

            TempInvoice = new Invoice(Main.ActualCashier);
            ActualCheckout = checkout;

            var tempList =
                new List<SaveableArticle>
                {
                    new SaveableArticle
                    {
                        Name = "Bananes",
                        Price = 1.50M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableArticle
                    {
                        Name = "Cacao",
                        Price = 1.50M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableArticle
                    {
                        Name = "Café",
                        Price = 1.50M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableArticle
                    {
                        Name = "Chocolat chaud",
                        Price = 1M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableArticle
                    {
                        Name = "Pommes",
                        Price = 1.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableArticle
                    {
                        Name = "Vin",
                        Price = 100.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableArticle
                    {
                        Name = "Patate",
                        Price = 1.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    },
                    new SaveableArticle
                    {
                        Name = "Poms",
                        Price = 1.8M,
                        ImageSrc = "pack://application:,,,/CaisseDesktop;component/Resources/Images/logo_brique.png"
                    }
                };

            Loaded += (sender, args) =>
            {
                if (checkout == null)
                {
                    CreateItemGrid(tempList);
                    return;
                }


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

        private string GetDefaultImageName(string typeName)
        {
            var type = 0;

            switch (typeName)
            {
                case "Alimentation":
                    type = 1;
                    break;
                case "Consignes":
                    type = 2;
                    break;
            }

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
                var nb = Math.Max(0, int.TryParse(textBox.Text, out var actualValue) ? actualValue - 1 : 0);
                textBox.Text = nb
                    .ToString();
                TempInvoice.RemoveBuyableItem(item, 1);
            };

            plusBtn.Click += (sender, args) =>
            {
                var nb = int.TryParse(textBox.Text, out var actualValue) ? actualValue + 1 : 0;
                textBox.Text = nb.ToString();
                TempInvoice.AddBuyableItem(item, 1);
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

        private void TestPrint_OnClick(object sender, RoutedEventArgs e)
        {
            var receipt = Main.ReceiptTicket.PrintWith(new Invoice(Main.ActualCashier)
            {
                GivenMoney = 50,
                Operations = new List<SaveableOperation>
                {
                    new SaveableOperation
                    {
                        Amount = 2,
                        Item = new SaveableArticle
                        {
                            Price = 2,
                            Name = "Crèpe"
                        }
                    },
                    new SaveableOperation
                    {
                        Amount = 1,
                        Item = new SaveableArticle
                        {
                            Price = 10,
                            Name = "Pizza"
                        }
                    }
                }
            });

            Main.TicketPrinter.Print(new List<ITicket> {receipt});
        }
    }
}