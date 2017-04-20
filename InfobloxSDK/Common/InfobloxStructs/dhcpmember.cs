using System;
using System.Net;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
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
                NetworkAddressTest.isIPv4WithExceptionAllowEmpty(value, out this._ipv4addr);
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
                NetworkAddressTest.isIPv6WithExceptionAllowEmpty(value, out this._ipv6addr);
            }
        }

        public string name { get; set; }
    }
}
