using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CaisseDesktop.Enums;
using CaisseDesktop.Graphics.Admin.TimeSlots;
using CaisseDesktop.Models;
using CaisseDesktop.Utils;
using CaisseLibrary.Utils;
using CaisseServer;
using CaisseServer.Events;
using CaisseServer.Items;

namespace CaisseDesktop.Graphics.Admin.Checkouts.Pages
{
    /// <summary>
    ///     Interaction logic for CheckoutTimeTablePage.xaml
    /// </summary>
    public partial class CheckoutTimeTablePage
    {
        private List<SaveableDay> Days { get; }

        private int PageIndex { get; set; }

        private bool CanClick { get; set; } = true;

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
                ForwardBtn.IsEnabled = true;

            var range = Days.Count > 3 ? 3 : Days.Count;

            if (range == 0)
                return;

            Task.Run(() => FillAll(false));
        }

        public override string CustomName => "CheckoutTimeTablePage";

        public string DateToHour(DateTime date)
        {
            return $"{date.Hour}h{(date.Minute < 10 ? $"0{date.Minute}" : date.Minute.ToString())}";
        }

        public override void Update()
        {
            FillAll(true);
        }

        public override void Add<T>(T item)
        {
        }

        public override bool CanClose() => true;

        public override bool CanBack() => true;

        public void Fill(TimeTableDay timeTableDay, SaveableDay day, List<SaveableTimeSlot> slots, bool set)
        {
            Panel panel;

            switch (timeTableDay)
            {
                case TimeTableDay.DayOne:
                    panel = Day1;
                    break;
                case TimeTableDay.DayTwo:
                    panel = Day2;
                    break;
                case TimeTableDay.DayThree:
                    panel = Day3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeTableDay), timeTableDay, null);
            }


            var color = System.Drawing.ColorTranslator.FromHtml(day.Color).Convert();
            var brush = new SolidColorBrush(color);

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

                dayBtn.Click += (sender, e) =>
                {
                    if (CanClick)
                        new TimeSlotManager(ParentWindow, null, day).ShowDialog();
                };

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
                        $"{DateToHour(slot.Start)}\n{(slot.Blank ? "Clique ici pour assigner la case." : slot.Cashier.GetFullName())}\n{DateToHour(slot.End)}",
                    Background = slot.Blank ? Brushes.Gray : brush,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Height = double.NaN,
                    DataContext = slot
                };

                dayBtn.Click += (sender, e) =>
                {
                    if (CanClick)
                        new TimeSlotManager(ParentWindow, slot, day).ShowDialog();
                };

                DockPanel.SetDock(dayBtn, Dock.Top);

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
            if (!CanClick) return;
            PageIndex++;

            if (PageIndex >= Days.Count - 3)
                ForwardBtn.IsEnabled = false;

            if (!BackBtn.IsEnabled)
                BackBtn.IsEnabled = true;

            Task.Run(() => FillAll(true));
        }

        private void BackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CanClick) return;
            PageIndex--;

            if (PageIndex <= 0)
                BackBtn.IsEnabled = false;

            if (!ForwardBtn.IsEnabled)
                ForwardBtn.IsEnabled = true;

            Task.Run(() => FillAll(true));
        }

        private void FillFromDb(TimeTableDay timeTableDay, SaveableDay day, bool set, CaisseServerContext db)
        {
            var timeSlots = db.TimeSlots.Where(t => t.Day.Id == day.Id).Include(t => t.Cashier)
                .Include(t => t.Substitute).Include(t => t.Checkout).Include(t => t.Day).ToList();
            Dispatcher.Invoke(() => { Fill(timeTableDay, day, timeSlots, set); });
        }

        private void FillAll(bool set)
        {
            Dispatcher.Invoke(() =>
            {
                CanClick = false;
                Mouse.OverrideCursor = Cursors.Wait;
            });

            using (var db = new CaisseServerContext())
            {
                FillFromDb(TimeTableDay.DayOne, Days[PageIndex], set, db);

                if (Days.Count > 1)
                    FillFromDb(TimeTableDay.DayTwo, Days[PageIndex + 1], set, db);
                if (Days.Count > 2)
                    FillFromDb(TimeTableDay.DayThree, Days[PageIndex + 2], set, db);
            }

            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                CanClick = true;
            });
        }
    }
}