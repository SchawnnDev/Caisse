using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin;

namespace CaisseDesktop.Utils
{
    public static class Parsers
    {

        public static CustomPage ToCustomPage(this Frame frame) => frame?.Content as CustomPage;

    }
}
