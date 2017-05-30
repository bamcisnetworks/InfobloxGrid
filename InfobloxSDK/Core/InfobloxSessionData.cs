using System.Net;

namespace BAMCIS.Infoblox.Core
{
    public static class InfobloxSessionData
    {
        public static readonly string LATEST = "LATEST";

        public static Cookie Cookie { get; set; }
        public static InfobloxCredential Credential { get; set; }
        public static string Version = LATEST;
        public static string GridMaster { get; set; }
        public static bool UseSessionData = false;

        public static void Reset()
        {
            Cookie = null;
            Credential = null;
            Version = LATEST;
            GridMaster = string.Empty;
            UseSessionData = false;
        }
    }
}
