using System.Media;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace CaisseDesktop.Graphics.Admin
{
    public abstract class CustomPage : Page
    {
        public abstract string CustomName { get; }

        public abstract void Update();

        public abstract void Add<T>(T item); // in case of a list page

        public abstract bool CanClose();

        public abstract bool CanBack();

        public bool Equals(CustomPage page)
        {
            return Equals(page?.CustomName);
        }

        public bool Equals(string name)
        {
            return name != null && CustomName.Equals(name);
        }

        public static bool Check(DateTimePicker picker)
        {
            var date = picker.Value;
            if (date != null) return false;
            picker.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
        }

        public static bool Check(TextBox box)
        {
            var str = box.Text;
            if (!string.IsNullOrWhiteSpace(str)) return false;
            box.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
        }

        public static bool Check(ComboBox box)
        {
            if (box.SelectedIndex != -1) return false;
            box.BorderBrush = Brushes.Red;
            SystemSounds.Beep.Play();
            return true;
        }

        public static bool Check(TextBlock block)
        {
            var str = block.Text;
            if (!string.IsNullOrWhiteSpace(str)) return false;
            //MessageBox.Show("Veuillez entrer une description valide.", "Erreur", MessageBoxButton.OK,MessageBoxImage.Error);
            SystemSounds.Beep.Play();
            return true;
        }
    }
}