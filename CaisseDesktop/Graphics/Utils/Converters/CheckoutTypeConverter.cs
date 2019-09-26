using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CaisseDesktop.Utils;
using CaisseLibrary.Enums;

namespace CaisseDesktop.Graphics.Utils.Converters
{
	[ValueConversion(typeof(int), typeof(string))]
	public class CheckoutTypeConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(string))
				throw new InvalidOperationException("The target must be a string");

			return ((CheckoutType)value).Description();
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}

