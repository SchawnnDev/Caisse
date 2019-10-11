using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Models.Admin.Days
{
    public class DayConfigModel : INotifyPropertyChanged
    {
        public readonly SaveableDay Day;
        public Dispatcher Dispatcher { get; set; }
        public bool IsCreating;
        private readonly EventManagerModel _parentModel;
        public Action CloseAction { get; set; }

        public DayConfigModel(EventManagerModel model, SaveableDay day)
        {
            _parentModel = model;
            IsCreating = day == null;
            Day = day ?? new SaveableDay {Event = model.SaveableEvent};
            Start = model.SaveableEvent.Start;
            End = model.SaveableEvent.End.AddDays(1);

            FirstDateTime = _parentModel.SaveableEvent.Start;
            LastDateTime = _parentModel.SaveableEvent.End;
        }

        private ICommand _saveCommand;
        private DateTime _firstDateTime;
        private DateTime _lastDateTime;

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));

        public DateTime LastDateTime
        {
            get => _lastDateTime;
            set
            {
                _lastDateTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime FirstDateTime
        {
            get => _firstDateTime;
            set
            {
                _firstDateTime = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get => Day.Color;
            set
            {
                Day.Color = value;
                OnPropertyChanged();
            }
        }

        public DateTime Start
        {
            get => Day.Start;
            set
            {
                Day.Start = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartTime
        {
            get => Day.Start;
            set
            {
                Day.Start = new DateTime(Day.Start.Year, Day.Start.Month, Day.Start.Day, value.Hour, value.Minute,
                    value.Second);
                OnPropertyChanged();
            }
        }

        public DateTime End
        {
            get => Day.End;
            set
            {
                Day.End = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndTime
        {
            get => Day.End;
            set
            {
                Day.End = new DateTime(Day.End.Year, Day.End.Month, Day.End.Day, value.Hour, value.Minute,
                    value.Second);
                OnPropertyChanged();
            }
        }

        private void Save(object arg)
        {
            if (Start.CompareTo(_parentModel.SaveableEvent.Start) < 0)
            {
                MessageBox.Show("La date de début du jour ne peut pas être avant la date de début de l'évenement!");
                return;
            }

            if (End.CompareTo(_parentModel.SaveableEvent.End) > 0)
            {
                MessageBox.Show("La date de fin du jour ne peut pas être après la date de fin de l'évenement!");
                return;
            }

            if (Start.CompareTo(End) >= 0)
            {
                MessageBox.Show("La date de début ne peut pas être après la date de fin!");
                return;
            }

            using (var db = new CaisseServerContext())
            {
                var days = db.Days.Where(t => t.Event.Id == _parentModel.SaveableEvent.Id).ToList();

                foreach (var day in days)
                {
                    // < 0 : date est avant valeur
                    // == 0 : date est égale valeur
                    // > 0 date est après valeur

                    if (Day.Id != 0 && Day.Id == day.Id) continue; // no duplicates :D

                    if (Start.CompareTo(day.Start) < 0 || End.CompareTo(day.Event) > 0)
                        continue;
                    if (Start.CompareTo(day.Start) > 0 || End.CompareTo(day.Start) < 0 || End.CompareTo(day.Event) > 0)
                        continue;
                    if (Start.CompareTo(day.Start) < 0 || Start.CompareTo(day.End) > 0 || End.CompareTo(day.Event) < 0)
                        continue;
                    if (MessageBox.Show(
                            "Le jour chevauche un autre jour déjà enregistré, es-tu sûr de vouloir sauvegarder ?",
                            "Jour chevauche un autre.", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                        continue;

                    Task.Run(Save);
                    return;
                }

                Task.Run(Save);
            }
        }

        private void Save()
        {
            Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

            using (var db = new CaisseServerContext())
            {
                db.Events.Attach(Day.Event);

                if (IsCreating)
                {
                    db.Days.Add(Day);
                }
                else
                {
                    db.Days.Attach(Day);
                    db.Days.AddOrUpdate(Day);
                }

                db.SaveChanges();
            }


            Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(IsCreating ? "Le jour a bien été crée !" : "Le jour a bien été enregistré !");

                CloseAction();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}