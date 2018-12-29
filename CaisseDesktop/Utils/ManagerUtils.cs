using System.Collections.Generic;
using System.Windows.Controls;
using CaisseDesktop.Graphics.Admin;

namespace CaisseDesktop.Utils
{
    public static class ManagerUtils
    {
        public static void DoPageNavigation(this List<MenuItem> items, int active)
        {
            if (items == null || items.Count == 0 || active >= items.Count) return;

            for (var i = 0; i < items.Count; i++)
                items[i].IsEnabled = i != active;
        }

        public static bool CanOpen(this CustomPage page, string other)
        {
            return page != null && !page.Equals(other);
        }
    }
}