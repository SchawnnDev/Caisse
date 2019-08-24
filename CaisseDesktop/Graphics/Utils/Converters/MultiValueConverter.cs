using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CaisseDesktop.Graphics.Utils.Converters
{
	public class MultiValueConverter : IMultiValueConverter
	{

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values.Clone();

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => value as object[];
	}
}
