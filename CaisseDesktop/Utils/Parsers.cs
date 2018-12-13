using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseDesktop.Graphics.Admin;

namespace CaisseDesktop.Utils
{
    public static class Parsers
    {

        public static CustomPage ToCustomPage(this object obj) => obj as CustomPage;

    }
}
