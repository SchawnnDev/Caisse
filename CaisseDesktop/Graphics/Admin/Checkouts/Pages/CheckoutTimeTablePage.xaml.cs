using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CaisseDesktop.Enums;
using CaisseLibrary.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    ///     Interaction logic for CheckoutTimeTablePage.xaml
    /// </summary>
    public partial class CheckoutTimeTablePage
    {

        private List<SaveableDay> Days {get;}

        private int StartRange { get; set; } = 1;

        public CheckoutTimeTablePage(CheckoutManager parentWindow)
        {
            InitializeComponent();

            using (var db = new CaisseServerContext())
            {

                Days = db.Days.Where(t=>t.Event.Id == parentWindow.ParentWindow.Evenement.Id).OrderByDescending(t=>t.Start).ToList();

            }

                /*

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
                    },
                    new SaveableTimeSlot
                    {
                        Day = day,
                        Start = new DateTime(2018, 12, 23, 14, 00, 00),
                        End = new DateTime(2018, 12, 23, 15, 00, 00),
                    },
                    new SaveableTimeSlot
                    {
                        Day = day,
                        Start = new DateTime(2018, 12, 23, 15, 00, 00),
                        End = new DateTime(2018, 12, 23, 16, 00, 00),
                    }
                }; */

                //  Fill(TimeTableDay.DayOne, day, list);


            var range = Days.Count > 3 ? 3 : Days.Count;

            if (range == 0)
            {
                return;
            }

            for (var i = 0; i < range; i++)
            {
                Fill((TimeTableDay)i, Days[i], new List<SaveableTimeSlot>());
            }

        }

        public override string CustomName => "CheckoutTimeTablePage";

        public string DateToHour(DateTime date)
        {
            return $"{date.Hour}h{(date.Minute < 10 ? $"0{date.Minute}" : date.Minute.ToString())}";
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
            return true;
        }

        public override bool CanBack()
        {
            return true;
        }

        public void Fill(TimeTableDay timeTableDay, SaveableDay day, List<SaveableTimeSlot> slots)
        {
            Panel panel;
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
                    Content = $"{DateToHour(day.Start)}\n\n\n\n{DateToHour(day.End)}",
                    HorizontalContentAlignment = HorizontalAlignment.Center,
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
                        $"{DateToHour(slot.Start)}\n{(slot.Blank ? "Clique ici pour assigner la case." : "Paul Meyer")}\n{DateToHour(slot.End)}",
                    Background = slot.Blank ? Brushes.Gray : brush,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Height = double.NaN,
                    DataContext = slot
                };

                //DockPanel.SetDock(dayBtn, Dock.Top);

                panel.DataContext = day;
                panel.Children.Add(dayBtn);
            }

            //RecalculateHeights(panel);
        }

        private void RecalculateHeights(DockPanel panel)
        {
            if (!(panel.Children[0] is Button dayBtn)) return;

            var height = panel.ActualHeight - dayBtn.ActualHeight;

            if (!(dayBtn.DataContext is SaveableDay day)) return;

            var timeInSeconds = (double) (day.End.ToUnixTimeStamp() - day.Start.ToUnixTimeStamp());

            for (var i = 1; i < panel.Children.Count; i++)
            {
                if (!(panel.Children[i] is Button child)) continue;
                if (!(child.DataContext is SaveableTimeSlot slot)) continue;
                var slotInSeconds = (double) (slot.End.ToUnixTimeStamp() - slot.Start.ToUnixTimeStamp());
                child.Height = height * (slotInSeconds / timeInSeconds);
            }
        }

        private List<SaveableTimeSlot> GenerateBlankSlots(SaveableDay day, List<SaveableTimeSlot> taken)
        {
            var blankSlots = new List<SaveableTimeSlot>();

            var dayStartHour = day.Start.Hour;
            var dayEndHour = day.End.Hour;
            var dayStartMinute = day.Start.Minute;
            var dayEndMinute = day.End.Minute;

            var min = taken.Min(t => t.Start);

            if (min.Hour == dayStartHour && min.Minute != dayStartMinute || min.Hour != dayStartHour)
                blankSlots.Add(new SaveableTimeSlot
                {
                    Start = day.Start,
                    End = min
                });

            for (var i = 1; i < taken.Count; i++)
            {
                var t1 = taken[i - 1].End;
                var t2 = taken[i].Start;

                if (t1.Hour == t2.Hour && t1.Minute != t2.Hour || t1.Hour != t2.Hour)
                    blankSlots.Add(new SaveableTimeSlot
                    {
                        Start = t1,
                        End = t2
                    });
            }


            var max = taken.Max(t => t.End);

            if (max.Hour == dayEndHour && max.Minute != dayEndMinute || max.Hour != dayEndHour)
                blankSlots.Add(new SaveableTimeSlot
                {
                    Start = max,
                    End = day.End
                });

            blankSlots.Select(t =>
            {
                t.Blank = true;
                return t;
            }).ToList();

            return blankSlots;
        }
    }
}