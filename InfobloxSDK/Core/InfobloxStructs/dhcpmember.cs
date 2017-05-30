using System;
using System.Net;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class dhcpmember
    {
        private string _ipv4addr;
        private string _ipv6addr;

        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv4Address(value, out IP, true, true))
                {
                    this._ipv4addr = IP.ToString();
                }
            }
        }

        public string ipv6addr
        {
            get
            {
                return this._ipv6addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv6Address(value, out IP, true, true))
                {
                    this._ipv6addr = IP.ToString();
                }
            }
        }

        public string name { get; set; }
    }
}
