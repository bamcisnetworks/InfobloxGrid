using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;

namespace BAMCIS.Infoblox.InfobloxObjects.IPAM
{
    [Name("ipv4addr")]
    public class ipv4addr : BaseIPAMIPAddress
    {
        private string _ip_address;
        private string _mac_address;
        private string _network;

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string dhcp_client_identifier { get; internal protected set; }
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
                NetworkAddressTest.isIPv4WithExceptionAllowEmpty(value, out this._ip_address);
            }
        }
        [ReadOnlyAttribute]
        public bool is_invalid_mac { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string mac_address 
        {
            get
            {
                return this._mac_address;
            }
            internal protected set
            {
                NetworkAddressTest.IsMACWithExceptionAllowEmpty(value, out this._mac_address);
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
        public string status { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string username { get; internal protected set; }
    }
}
