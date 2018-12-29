using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin;

namespace CaisseDesktop.Utils
{
    public static class Parsers
    {
        public static CustomPage ToCustomPage(this Frame frame)
        {
            return frame?.Content as CustomPage;
        }
    }
}