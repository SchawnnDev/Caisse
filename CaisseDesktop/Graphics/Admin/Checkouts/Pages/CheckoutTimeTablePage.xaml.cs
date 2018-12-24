using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaisseDesktop.Enums;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    /// Interaction logic for CheckoutTimeTablePage.xaml
    /// </summary>
    public partial class CheckoutTimeTablePage
    {
        public CheckoutTimeTablePage()
        {
            InitializeComponent();

            var day = new SaveableDay
            {
                Start = new DateTime(2018, 12, 23, 08, 01, 00),
                End = new DateTime(2018, 12, 23, 17, 01, 00)
            };

            var day2 = new SaveableDay
            {
                Start = new DateTime(2018, 12, 24, 08, 00, 00),
                End = new DateTime(2018, 12, 24, 17, 00, 00)
            };

            var day3 = new SaveableDay
            {
                Start = new DateTime(2018, 12, 25, 08, 00, 00),
                End = new DateTime(2018, 12, 25, 17, 00, 00)
            };

            var list = new List<SaveableTimeSlot>
            {
                new SaveableTimeSlot
                {
                    Day = day,
                    Start = new DateTime(2018, 12, 23, 10, 00, 00),
                    End = new DateTime(2018, 12, 23, 12, 00, 00)
                },
                new SaveableTimeSlot
                {
                    Day = day,
                    Start = new DateTime(2018, 12, 23, 14, 01, 00),
                    End = new DateTime(2018, 12, 23, 15, 01, 00)
                },
                new SaveableTimeSlot
                {
                    Day = day,
                    Start = new DateTime(2018, 12, 23, 15, 00, 00),
                    End = new DateTime(2018, 12, 23, 16, 00, 00)
                },
            };

            Fill(TimeTableDay.DayOne, day, list);
            Fill(TimeTableDay.DayTwo, day2, new List<SaveableTimeSlot>());
            Fill(TimeTableDay.DayThree, day3, new List<SaveableTimeSlot>());

        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Add<T>(T item)
        {
            throw new NotImplementedException();
        }

        public override bool CanClose()
        {
            throw new NotImplementedException();
        }

        public override bool CanBack()
        {
            throw new NotImplementedException();
        }

        public void Fill(TimeTableDay timeTableDay, SaveableDay day, List<SaveableTimeSlot> slots)
        {
            DockPanel panel;
            Brush brush;
            switch (timeTableDay)
            {
                case TimeTableDay.DayOne:
                    panel = Day1;
                    brush = Brushes.DarkSeaGreen;
                    break;
                case TimeTableDay.DayTwo:
                    panel = Day2;
                    brush = Brushes.CornflowerBlue;
                    break;
                case TimeTableDay.DayThree:
                    panel = Day3;
                    brush = Brushes.DarkSalmon;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeTableDay), timeTableDay, null);
            }

            //TimeTableGrid.ColumnDefinitions[1].

            var label = new Button
            {
                Content = day.Start.ToString("D"),
                Background = brush
            };

            DockPanel.SetDock(label, Dock.Top);
            panel.Children.Add(label);

            if (slots.Count == 0)
            {
                var dayBtn = new Button
                {
                    Content = $"{day.Start.Date:T}\n\n\n{day.Start.Date:d}\n\n\n{day.End.Date:T}",
                    Background = brush,
                    Height = double.NaN
                };

                DockPanel.SetDock(dayBtn, Dock.Top);

                panel.DataContext = day;
                panel.Children.Add(dayBtn);

                return;

            }

            foreach (var slot in slots)
            {
                var dayBtn = new Button
                {
                    Content = $"{slot.Start.Date:T}\n\n\n{slot.End.Date:T}",
                    Background = brush,
                    Height = double.NaN
                };

                DockPanel.SetDock(dayBtn, Dock.Top);

                panel.DataContext = day;
                panel.Children.Add(dayBtn);
            }

        }

        public override string CustomName => "CheckoutTimeTablePage";
    }
}
