using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using BAMCIS.Infoblox.Common.Enums;

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
                NetworkAddressTest.IsIPv6DUIDWithExceptionAllowEmpty(value, out this._duid);
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
                NetworkAddressTest.isIPv6WithExceptionAllowEmpty(value, out this._ip_address);
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
                NetworkAddressTest.IsIPv4CidrWithExceptionAllowEmpty(value, out this._network);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public IPv6IPAMAddressStatusEnum status { get; internal protected set; }

    }
}
