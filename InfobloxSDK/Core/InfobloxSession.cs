using System.Net;

namespace BAMCIS.Infoblox.Core
{
    public class InfobloxSession
    {
        public Cookie Cookie { get; set; }
        public InfobloxCredential Credential { get; set; }
        public string Version { get; set; }
        public string GridMaster { get; set; }
    }
}
