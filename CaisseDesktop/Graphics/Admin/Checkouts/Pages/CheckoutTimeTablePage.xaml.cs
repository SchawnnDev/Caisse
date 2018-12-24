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
using CaisseLibrary.Utils;
using CaisseServer;
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
                Start = new DateTime(2018, 12, 23, 08, 00, 00),
                End = new DateTime(2018, 12, 23, 17, 00, 00)
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
                    End = new DateTime(2018, 12, 23, 12, 00, 00),
                    Cashier = new SaveableCashier
                    {
                        FirstName = "Felix",
                        Name = "Meyer"
                    }
                },
                new SaveableTimeSlot
                {
                    Day = day,
                    Start = new DateTime(2018, 12, 23, 14, 00, 00),
                    End = new DateTime(2018, 12, 23, 15, 00, 00),
                    Cashier = new SaveableCashier
                    {
                        FirstName = "Paul",
                        Name = "Meyer"
                    }
                },
                new SaveableTimeSlot
                {
                    Day = day,
                    Start = new DateTime(2018, 12, 23, 15, 00, 00),
                    End = new DateTime(2018, 12, 23, 16, 00, 00),
                    Cashier = new SaveableCashier
                    {
                        FirstName = "Thierry",
                        Name = "Meyer"
                    }
                },
            };

            Fill(TimeTableDay.DayOne, day, list);
            Fill(TimeTableDay.DayTwo, day2, new List<SaveableTimeSlot>());
            Fill(TimeTableDay.DayThree, day3, new List<SaveableTimeSlot>());
            
        }

        public string DateToHour(DateTime date) =>
            $"{date.Hour}h{(date.Minute < 10 ? $"0{date.Minute}" : date.Minute.ToString())}";

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
                    Content = $"{DateToHour(day.Start)}\n\n\n\n\n\n{DateToHour(day.End)}",
                    Background = brush,
                    Height = double.NaN
                };

                DockPanel.SetDock(dayBtn, Dock.Top);

                panel.DataContext = day;
                panel.Children.Add(dayBtn);

                return;
            }

            slots.AddRange(GenerateBlankSlots(day, slots));

            slots = slots.OrderBy(t => t.Start).ToList();

            foreach (var slot in slots)
            {
                var dayBtn = new Button
                {
                    Content =
                        $"{DateToHour(slot.Start)}\n\n\n{(slot.Blank ? "Clique ici pour assigner la case." : slot.Cashier.GetFullName())}\n\n\n{DateToHour(slot.End)}",
                    Background = slot.Blank ? Brushes.Gray : brush,
                    Height = double.NaN,
                    DataContext = slot
                };

                DockPanel.SetDock(dayBtn, Dock.Top);

                panel.DataContext = day;
                panel.Children.Add(dayBtn);
            }

            RecalculateHeights(panel);
        }

        private void RecalculateHeights(DockPanel panel)
        {
            if (!(panel.Children[0] is Button dayBtn)) return;

            var height = panel.ActualHeight - dayBtn.ActualHeight;

            if (!(dayBtn.DataContext is SaveableDay day)) return;

            var timeInSeconds =(double) (day.End.ToUnixTimeStamp() - day.Start.ToUnixTimeStamp());

            for (var i = 1; i < panel.Children.Count; i++)
            {
                if (!(panel.Children[i] is Button child)) continue;
                if (!(child.DataContext is SaveableTimeSlot slot)) continue;
                var slotInSeconds = (double) (slot.End.ToUnixTimeStamp() - slot.Start.ToUnixTimeStamp());
                child.Height = height * (slotInSeconds / timeInSeconds);
            }

        }

        public override string CustomName => "CheckoutTimeTablePage";

        private List<SaveableTimeSlot> GenerateBlankSlots(SaveableDay day, List<SaveableTimeSlot> taken)
        {
            var blankSlots = new List<SaveableTimeSlot>();

            var dayStartHour = day.Start.Hour;
            var dayEndHour = day.End.Hour;
            var dayStartMinute = day.Start.Minute;
            var dayEndMinute = day.End.Minute;

            var min = taken.Min(t => t.Start);

            if (min.Hour == dayStartHour && min.Minute != dayStartMinute || min.Hour != dayStartHour)
            {
                blankSlots.Add(new SaveableTimeSlot
                {
                    Start = day.Start,
                    End = min
                });
            }

            var max = taken.Max(t => t.End);

            if (max.Hour == dayEndHour && max.Minute != dayEndMinute || max.Hour != dayEndHour)
            {
                blankSlots.Add(new SaveableTimeSlot
                {
                    Start = max,
                    End = day.End
                });
            }

            blankSlots.Select(t =>
            {
                t.Blank = true;
                return t;
            }).ToList();

            return blankSlots;
        }
    }
}