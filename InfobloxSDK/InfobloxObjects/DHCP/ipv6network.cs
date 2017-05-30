using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;
using System.Collections.Generic;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("ipv6network")]
    public class ipv6network : BaseIPv4andIPv6Network
    {
        private string _network;
        private string _domain_name;
        private List<string> _domain_name_servers;


        public bool ddns_enable_option_fqdn { get; set; }
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
                if (value.Length > 0)
                {
                    this._domain_name_servers = new List<string>();

                    foreach (string val in value)
                    {
                        if (NetworkAddressTest.IsFqdn(val))
                        {
                            this._domain_name_servers.Add(val);
                        }
                        else
                        {
                            throw new ArgumentException("Each domain name server must be a valid FQDN.");
                        }
                    }
                }
            }
        }
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
        public uint preferred_lifetime { get; set; }
        public bool use_ddns_enable_option_fqdn { get; set; }
        public bool use_domain_name { get; set; }
        public bool use_domain_name_servers { get; set; }
        public bool use_preferred_lifetime { get; set; }
        public bool use_valid_lifetime { get; set; }
        public uint valid_lifetime { get; set; }

        public ipv6network(string network)
        {
            this.ddns_enable_option_fqdn = false;
            this.domain_name = String.Empty;
            this.domain_name_servers = new string[0];
            this.network = network;
            this.preferred_lifetime = 27000;
            this.use_ddns_enable_option_fqdn = false;
            this.use_domain_name = false;
            this.use_domain_name_servers = false;
            this.use_valid_lifetime = false;
            this.valid_lifetime = 43200;
        }
    }
}
