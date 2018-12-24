using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseLibrary.Utils
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this ulong unixTimeStamp) => new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeStamp);

        public static ulong ToUnixTimeStamp(this DateTime dateTime) => Convert.ToUInt64(dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
    }
}
