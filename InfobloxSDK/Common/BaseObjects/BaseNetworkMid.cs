using System;

namespace BAMCIS.Infoblox.Common.BaseObjects
{
    public abstract class BaseNetworkMid : BaseNetworkLow
    {
        public string ddns_domainname { get; set; }
        public bool ddns_use_option81 { get; set; }
        public bool use_ddns_domainname { get; set; }
        public bool use_ddns_use_option81 { get; set; }
        
        public BaseNetworkMid()
        {
            this.ddns_domainname = String.Empty;
            this.ddns_use_option81 = false;
            this.use_ddns_domainname = false;
            this.use_ddns_use_option81 = false;
        }
    }
}
