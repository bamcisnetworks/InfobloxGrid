using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System.Net;

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
        [Basic]
        public string dhcp_client_identifier { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        [Basic]
        public string ip_address 
        {
            get
            {
                return this._ip_address;
            }
            internal protected set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv4Address(value, out IP, true, true))
                {
                    this._ip_address = IP.ToString();
                }
            }
        }

        [ReadOnlyAttribute]
        public bool is_invalid_mac { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string mac_address 
        {
            get
            {
                return this._mac_address;
            }
            internal protected set
            {
                NetworkAddressTest.IsMAC(value, out this._mac_address, true, true);
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public string network 
        { 
            get
            {
                return this._network;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPv4Cidr(value, out this._network, true, true);
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public string status { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string username { get; internal protected set; }
    }
}
