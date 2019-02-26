using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CaisseDesktop.Models
{
    public class DayPickerModel : INotifyPropertyChanged
    {
        private DateTime _start;
        private DateTime _end;

        public DayPickerModel()
        {
            Start = DateTime.Now;
            End = DateTime.Now;
        }

        public DateTime Start
        {
            get => _start;
            set
            {
                _start = value;
                OnPropertyChanged();
            }
        }

        public DateTime End
        {
            get => _end;
            set
            {
                _end = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}