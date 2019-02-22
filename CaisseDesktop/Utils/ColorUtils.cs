using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CaisseDesktop.Utils
{
    public static class ColorUtils
    {
        public static Color Convert(this System.Drawing.Color color) => Color.FromArgb(color.A,color.R,color.G,color.B);
    }
}
