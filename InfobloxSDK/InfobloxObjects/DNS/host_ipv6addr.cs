using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using System;
using System.Collections.Generic;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:host_ipv6addr")]
    public class host_ipv6addr : BaseHostRecord
    {
        private string _domain_name;
        private List<string> _domain_name_servers;
        private string _duid;
        private string _ipv6addr;
        private string _ipv6prefix;

        public IPv6AddressTypeEnum address_type { get; set; }

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

        public string[] domain_name_servers
        {
            get
            {
                return this._domain_name_servers.ToArray();
            }
            set
            {
                this._domain_name_servers = new List<string>();

                foreach (string item in value)
                {
                    IPAddress IP;
                    if (NetworkAddressTest.IsIPv6Address(item, out IP, false, true))
                    {
                        this._domain_name_servers.Add(IP.ToString());
                    }
                }
            }
        }

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
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

        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
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

        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv6prefix
        {
            get
            {
                return this._ipv6prefix;
            }
            set
            {
                IPAddress IP;

                if (NetworkAddressTest.IsIPv6Address(value, out IP, true, true))
                {
                    this._ipv6prefix = IP.ToString();
                }
            }
        }

        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        public uint ipv6prefix_bits { get; set; }

        public IPv6MatchClientEnum match_client { get; set; }

        public uint preferred_lifetime { get; set; }

        public bool use_domain_name { get; set; }

        public bool use_domain_servers { get; set; }

        public bool use_preferred_lifetime { get; set; }

        public bool use_valid_lifetime { get; set; }

        public uint valid_lifetime { get; set; }

        public host_ipv6addr()
        {
            this.address_type = IPv6AddressTypeEnum.ADDRESS;
            this.configure_for_dhcp = true;
            this.domain_name = String.Empty;
            this.domain_name_servers = new string[0];
            this.match_client = IPv6MatchClientEnum.DUID;
            this.options = dhcpoption.DefaultArray();
            this.preferred_lifetime = 27000;
            this.reserved_interface = String.Empty;
            this.use_domain_name = false;
            this.use_domain_servers = false;
            this.use_for_ea_inheritance = false;
            this.use_options = false;
            this.use_preferred_lifetime = false;
            this.use_valid_lifetime = false;
            this.valid_lifetime = 43200;
        }
    }
}
