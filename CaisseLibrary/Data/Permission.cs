using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseLibrary.Data
{
    public class Permission
    {
        public string Value { get; set; }

        public Permission(string value)
        {
            Value = value;
        }
    }
}