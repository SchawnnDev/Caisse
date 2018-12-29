using System;

namespace CaisseLibrary.Utils
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this ulong unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeStamp);
        }

        public static ulong ToUnixTimeStamp(this DateTime dateTime)
        {
            return Convert.ToUInt64(dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
    }
}