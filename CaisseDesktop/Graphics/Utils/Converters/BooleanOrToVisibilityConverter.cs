using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CaisseDesktop.Graphics.Utils.Converters
{
    public class BooleanAndToVisibilityConverter : IMultiValueConverter
    {
        public Visibility HiddenVisibility { get; set; }

        public bool IsInverted { get; set; }

        public BooleanAndToVisibilityConverter()
        {
            HiddenVisibility = Visibility.Collapsed;
            IsInverted = false;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = values.OfType<IConvertible>().All(System.Convert.ToBoolean);
            if (IsInverted) flag = !flag;
            return flag ? Visibility.Visible : HiddenVisibility;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
