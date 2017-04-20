using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Discovery
{
    public class networkdeprovisioninfo
    {
        private string _network;

        [NotReadableAttribute]
        public string @interface { get; set; }
        [NotReadableAttribute]
        public string network
        {
            get
            {
                return this._network;
            }
            set
            {
                if (NetworkAddressTest.IsIPv4Cidr(value) || NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new ArgumentException("The provided network value was not a valid IPv4 or IPv6 CIDR notation.");
                }
            }
        }
        [NotReadableAttribute]
        public string network_view { get; set; }

    }
}
