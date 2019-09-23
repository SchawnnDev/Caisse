using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using CaisseDesktop.Utils;

namespace CaisseDesktop.Graphics.Utils.Converters
{
	[ValueConversion(typeof(Enum), typeof(IEnumerable<ValueDescription>))]
	public class EnumToCollectionConverter : MarkupExtension, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Extensions.GetAllValuesAndDescriptions(value.GetType());

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

		public override object ProvideValue(IServiceProvider serviceProvider) => this;
	}
}