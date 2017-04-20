using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("ipv6networkcontainer")]
    public class ipv6networkcontainer : BaseNetworkContainer
    {
        private string _network;

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string network
        {
            get
            {
                return this._network;
            }
            set
            {
                if (NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new ArgumentException("The network must be a valid IPv6/CIDR format.");
                }
            }
        }

        public ipv6networkcontainer(string network)
        {
            this.network = network;
        }
    }
}
