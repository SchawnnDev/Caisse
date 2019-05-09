﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CaisseDesktop.Utils.XamlExtensions.Converters
{
	[ValueConversion(typeof(bool?), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool?))
			{
				throw new InvalidOperationException("The target must be a nullable boolean");
			}
			var b = (bool?)value;
			return b.HasValue && !b.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !(value as bool?);
		}

		#endregion
	}
}
