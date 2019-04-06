using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CaisseDesktop.Enums;
using CaisseDesktop.Graphics.Admin.TimeSlots;
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
        private List<SaveableDay> Days { get; }

        private int PageIndex { get; set; }

		private CheckoutManager ParentWindow { get; set; }

        public CheckoutTimeTablePage(CheckoutManager parentWindow)
        {
            InitializeComponent();
	        ParentWindow = parentWindow;

            using (var db = new CaisseServerContext())
            {
                Days = db.Days.Where(t => t.Event.Id == parentWindow.ParentWindow.Evenement.Id)
                    .OrderBy(t => t.Start).ToList();
            }

            if (Days.Count > 3)
            {
                ForwardBtn.IsEnabled = true;
            }

            var range = Days.Count > 3 ? 3 : Days.Count;

            if (range == 0)
            {
                return;
            }

            for (var i = 0; i < range; i++)
            {
                Fill((TimeTableDay) i, Days[i], new List<SaveableTimeSlot>(), false);
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

        public void Fill(TimeTableDay timeTableDay, SaveableDay day, List<SaveableTimeSlot> slots, bool set)
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


            var label = new Button
            {
                Content = day.Start.ToString("D"),
                Background = brush
            };

            DockPanel.SetDock(label, Dock.Top);

            if (set)
                panel.Children.Clear();

            panel.Children.Add(label);

            if (slots.Count == 0)
            {
                var dayBtn = new Button
                {
                    Content = $"{DateToHour(day.Start)}\n\n\n\n{DateToHour(day.End)}",
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Background = brush,
                    Height = double.NaN,
                };

                DockPanel.SetDock(dayBtn, Dock.Top);

                dayBtn.Click += (sender, e) => { new TimeSlotManager(null, day, ParentWindow.Checkout, day.Start, day.End).ShowDialog(); };

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

        private void ForwardBtn_OnClick(object sender, RoutedEventArgs e)
        {
            PageIndex++;

            if (PageIndex >= Days.Count - 3)
                ForwardBtn.IsEnabled = false;

            if (!BackBtn.IsEnabled)
                BackBtn.IsEnabled = true;

            FillAll();
        }

        private void BackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            PageIndex--;

            if (PageIndex <= 0)
                BackBtn.IsEnabled = false;

            if (!ForwardBtn.IsEnabled)
                ForwardBtn.IsEnabled = true;

            FillAll();
        }

        private void FillAll()
        {
            Fill(TimeTableDay.DayOne, Days[PageIndex], new List<SaveableTimeSlot>(), true);
            Fill(TimeTableDay.DayTwo, Days[PageIndex + 1], new List<SaveableTimeSlot>(), true);
            Fill(TimeTableDay.DayThree, Days[PageIndex + 2], new List<SaveableTimeSlot>(), true);
        }
    }
}