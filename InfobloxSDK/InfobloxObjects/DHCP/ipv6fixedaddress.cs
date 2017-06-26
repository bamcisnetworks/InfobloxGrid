using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("ipv6fixedaddress")]
    public class ipv6fixedaddress : BaseFixedAddressObject
    {
        private string _domain_name;
        private List<string> _domain_name_servers;
        private string _duid;
        private string _ipv6addr;
        private string _ipv6prefix;
        private string _network;

        [SearchableAttribute(Equality = true)]
        public IPv6AddressTypeEnum address_type { get; set; }

        public string domain_name
        {
            get
            {
                return this._domain_name;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "domain_name", out this._domain_name, false, true);
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
                foreach(string val in value)
                {
                    IPAddress ip;
                    if (NetworkAddressTest.IsIPv6Address(val, out ip))
                    {
                        this._domain_name_servers.Add(ip.ToString());
                    }
                    else
                    {
                        throw new ArgumentException("All domain name server addresses must be a valid IPv6 address.");
                    }
                }
            }
        }

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string duid
        {
            get
            {
                return this._duid;
            }
            set
            {
                NetworkAddressTest.IsIPv6DUID(value, out this._duid, false, true);
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
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    this._ipv6addr = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The IPv6 address must be valid.");
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
                if (NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._ipv6prefix = value;
                }
                else
                {
                    throw new ArgumentException("The IPv6 prefix must be a valid IPv6/CIDR format.");
                }
            }
        }

        [SearchableAttribute(Equality = true)]
        public uint ipv6prefix_bits { get; set; }

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
                    throw new ArgumentException("The network must be a a valid IPv6/CIDR format.");
                }
            }
        }

        public uint preferred_lifetime { get; set; }
        public string reserved_interface { get; set; }
        public bool use_domain_name { get; set; }
        public bool use_domain_name_servers { get; set; }
        public bool use_preferred_lifetime { get; set; }
        public bool use_valid_lifetime { get; set; }
        public uint valid_lifetime { get; set; }

        public ipv6fixedaddress(string duid, string ipv6addr)
        {
            this.address_type = IPv6AddressTypeEnum.ADDRESS;
            this.domain_name = String.Empty;
            this.domain_name_servers = new string[0];
            this.duid = duid;
            this.ipv6addr = ipv6addr;
            this.preferred_lifetime = 27000;
            this.reserved_interface = String.Empty;
            this.use_domain_name = false;
            this.use_domain_name_servers = false;
            this.use_preferred_lifetime = false;
            this.use_valid_lifetime = false;
            this.valid_lifetime = 43200;
        }

        public ipv6fixedaddress(string duid, string ipv6prefix, uint ipv6prefixbits, IPv6AddressTypeEnum address_type)
        {
            if (address_type == IPv6AddressTypeEnum.BOTH || address_type == IPv6AddressTypeEnum.PREFIX)
            {
                this.address_type = address_type;
                this.domain_name = String.Empty;
                this.domain_name_servers = new string[0];
                this.duid = duid;
                this.ipv6prefix = ipv6prefix;
                this.ipv6prefix_bits = ipv6prefix_bits;
                this.preferred_lifetime = 27000;
                this.reserved_interface = String.Empty;
                this.use_domain_name = false;
                this.use_domain_name_servers = false;
                this.use_preferred_lifetime = false;
                this.use_valid_lifetime = false;
                this.valid_lifetime = 43200;
            }
            else
            {
                throw new ArgumentException("Use a different constructor to set the address type to \"ADDRESS\" and define an IPv6 address.");
            }
        }
    }
}
