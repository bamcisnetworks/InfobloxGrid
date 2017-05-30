using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("ipv6sharednetwork")]
    public class ipv6sharednetwork : BaseSharedNetwork
    {
        private string _domain_name;
        private string _name;

        public string domain_name
        {
            get
            {
                return this._domain_name;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "domain_name", out this._domain_name, true, true);
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.TrimValueWithException("name");
            }
        }
        [Required]
        public ipv6network[] networks { get; set; } //The network objects must contain a _ref property

        public ipv6sharednetwork(string name, ipv6network[] networks)
        {
            this.domain_name = String.Empty;
            this.name = name;
            this.networks = networks;
        }
    }
}
