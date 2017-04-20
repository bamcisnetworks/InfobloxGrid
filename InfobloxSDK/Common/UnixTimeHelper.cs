using System;

namespace BAMCIS.Infoblox.Common
{
    public static class UnixTimeHelper
    {
        public static DateTime FromUnixTime(long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
        }

        public static long ToUnixTime(DateTime timeStamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((timeStamp - epoch).TotalSeconds);
        }
    }
}
