using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.IPAM
{
    [Name("ipv6addr")]
    public class ipv6addr : BaseIPAMIPAddress
    {
        private string _duid;
        private string _ip_address;
        private string _network;
        
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string duid
        {
            get
            {
                return this._duid;
            }
            set
            {
                NetworkAddressTest.IsIPv6DUID(value, out this._duid, true, true);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        public string ip_address
        {
            get
            {
                return this._ip_address;
            }
            internal protected set
            {
                IPAddress IP;

                if (NetworkAddressTest.IsIPv6Address(value, out IP, true, true))
                {
                    this._ip_address = IP.ToString();
                }
            }
        }
        
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string network
        {
            get
            {
                return this._network;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPv6Cidr(value, out this._network, true, true);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public IPv6IPAMAddressStatusEnum status { get; internal protected set; }

    }
}
