using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CaisseDesktop.Graphics.Assets.Rules
{
    public class NumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var canConvert = int.TryParse(value as string, out _);
            return new ValidationResult(canConvert, "Not a valid int");
        }
    }
}
