using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace CaisseDesktop.Graphics.Admin
{
    public abstract class CustomPage : Page
    {

        public CustomPage()
        {
        }

        public abstract bool CanClose();

        public abstract bool CanBack();

        public abstract string CustomName { get; }

        public bool Equals(CustomPage page) => Equals(page?.CustomName);

        public bool Equals(string name) => name != null && CustomName.Equals(name);

        public bool Check(DateTimePicker picker)
        {
            var date = picker.Value;
            if (date != null) return false;
            picker.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
        }

        public bool Check(TextBox box)
        {
            var str = box.Text;
            if (!string.IsNullOrWhiteSpace(str)) return false;
            box.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
        }

        public bool Check(TextBlock block)
        {
            var str = block.Text;
            if (!string.IsNullOrWhiteSpace(str)) return false;
            //MessageBox.Show("Veuillez entrer une description valide.", "Erreur", MessageBoxButton.OK,MessageBoxImage.Error);
            SystemSounds.Beep.Play();
            return true;
        }
    }
}