using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace CaisseDesktop.Graphics.Assets
{
    public class BindablePasswordBox : ContentControl
    {
        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bindablePasswordBox = (BindablePasswordBox) d;
            bindablePasswordBox.OnPasswordPropertyChanged((string) e.OldValue, (string) e.NewValue);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox)
                , new FrameworkPropertyMetadata(string.Empty
                    , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
                    , BindablePasswordBox.OnPasswordPropertyChanged
                    , (d, v) => v ?? string.Empty as object
                    , true
                    , UpdateSourceTrigger.LostFocus
                )
            );

        public string Password
        {
            get { return (string) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private PasswordBox _passwordBox;
        private RoutedEventHandler _passwordChanged;
        private bool _isInsidePasswordContentChange;

        public BindablePasswordBox()
        {
            Content = new PasswordBox();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            Unsubscribe(oldContent);
            base.OnContentChanged(oldContent, newContent);
            Subscribe(newContent);
        }

        private void Subscribe(object newContent)
        {
            if (!(newContent is PasswordBox passwordBox)) return;
            _passwordBox = passwordBox;
            _passwordChanged = (sender, e) =>
            {
                _isInsidePasswordContentChange = true;
                this.Password = passwordBox.Password;
                _isInsidePasswordContentChange = false;
            };
            passwordBox.PasswordChanged += _passwordChanged; // subscribe event handler
        }

        private void Unsubscribe(object oldContent)
        {
            if (!(oldContent is PasswordBox passwordBox)) return;
            passwordBox.PasswordChanged -= _passwordChanged; // unsubscribe event handler
        }

        private void OnPasswordPropertyChanged(string oldValue, string newValue)
        {
            if (_isInsidePasswordContentChange)
                return;

            var passwordBox = _passwordBox;
            if (passwordBox == null) return;
            passwordBox.PasswordChanged -= _passwordChanged; // unsubscribe event handler
            passwordBox.Password = newValue;
            passwordBox.PasswordChanged += _passwordChanged; // resubscribe event handler
        }
    }
}