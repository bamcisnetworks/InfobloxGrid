using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class networkinfo
    {
        private string _network_str;

        public string network { get; set; }
        public string network_str
        {
            get
            {
                return this._network_str;
            }
            set
            {
                if (NetworkAddressTest.IsIPv4Cidr(value) || NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._network_str = value;
                }
                else
                {
                    throw new ArgumentException("The provided string for the network_str variable was not a valid IPv6 or IPv4 CIDR notation.");
                }
            }
        }
    }
}
