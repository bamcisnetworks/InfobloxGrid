using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using System;
using System.Collections.Generic;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("ipv6networktemplate")]
    public class ipv6networktemplate : BaseNameCommentObject
    {
        private uint _cidr;
        private string _ddns_domainname;
        private string _domain_name;
        private List<string> _domain_name_servers;
        private string _ipv6prefix;

        public bool allow_any_network { get; set; }
        public bool auto_create_reversezone { get; set; }
        public uint cidr
        {
            get
            {
                return this._cidr;
            }
            set
            {
                if (value >= 24 && value <= 128)
                {
                    this._cidr = value;
                }
                else
                {
                    throw new ArgumentException("The IPv6 CIDR must be between 24 and 128.");
                }
            }
        }
        public bool cloud_api_compatible { get; set; }
        public string ddns_domainname
        {
            get
            {
                return this._ddns_domainname;
            }
            set
            {
                if (NetworkAddressTest.IsFqdn(value))
                {
                    this._ddns_domainname = value;
                }
                else
                {
                    throw new ArgumentException("The DDNS domain name must be a valid FQDN.");
                }
            }
        }
        public bool ddns_enable_option_fqdn { get; set; }
        public bool ddns_generate_hostname { get; set; }
        public bool ddns_server_always_updates { get; set; }
        public uint ddns_ttl { get; set; }
        public dhcpmember delegated_member { get; set; }
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
                        IPAddress ip;
                        if (NetworkAddressTest.IsIPv6Address(val, out ip))
                        {
                            this._domain_name_servers.Add(ip.ToString());
                        }
                        else
                        {
                            throw new ArgumentException("The domain name servers must be valid IPv6 addresses.");
                        }
                    }
                }
            }
        }
        public bool enable_ddns { get; set; }
        public string[] fixed_address_templates { get; set; }
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv6prefix
        {
            get
            {
                return this._ipv6prefix;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    IPAddress ip;
                    if (NetworkAddressTest.IsIPv6Address(value, out ip))
                    {
                        this._ipv6prefix = ip.ToString();
                    }
                    else
                    {
                        throw new ArgumentException("The IPv6 prefix must be a valid IPv6 address.");
                    }
                }
            }
        }
        public dhcpmember[] members { get; set; }
        public dhcpoption[] options { get; set; }
        public uint preferred_lifetime { get; set; }
        public string[] range_templates { get; set; }
        public bool recycle_leases { get; set; }
        public bool update_dns_on_lease_renewal { get; set; }
        public bool use_ddns_domainname { get; set; }
        public bool use_ddns_enable_option_fqdn { get; set; }
        public bool use_ddns_generate_hostname { get; set; }
        public bool use_ddns_ttl { get; set; }
        public bool use_domain_name { get; set; }
        public bool use_domain_name_servers { get; set; }
        public bool use_enable_ddns { get; set; }
        public bool use_options { get; set; }
        public bool use_preferred_lifetime { get; set; }
        public bool use_recycle_leases { get; set; }
        public bool use_update_dns_on_lease_renewal { get; set; }
        public bool use_valid_lifetime { get; set; }
        public uint valid_lifetime { get; set; }

        public ipv6networktemplate(string name)
        {
            this.allow_any_network = false;
            this.auto_create_reversezone = false;
            this.cloud_api_compatible = false;
            this.comment = String.Empty;
            this.ddns_enable_option_fqdn = false;
            this.ddns_generate_hostname = false;
            this.ddns_server_always_updates = true;
            this.ddns_ttl = 0;
            this.domain_name = String.Empty;
            this.domain_name_servers = new string[0];
            this.enable_ddns = false;
            this.fixed_address_templates = new string[0];
            this.ipv6prefix = String.Empty;
            this.members = new dhcpmember[0];
            this.name = name;
            this.options = new dhcpoption[] { new dhcpoption() { name = "dhcp-lease-time", num = 51, value = "43200", vendor_class = "DHCP" } };
            this.preferred_lifetime = 27000;
            this.range_templates = new string[0];
            this.recycle_leases = true;
            this.update_dns_on_lease_renewal = false;
            this.use_ddns_domainname = false;
            this.use_ddns_enable_option_fqdn = false;
            this.use_ddns_generate_hostname = false;
            this.use_ddns_ttl = false;
            this.use_domain_name = false;
            this.use_domain_name_servers = false;
            this.use_enable_ddns = false;
            this.use_options = false;
            this.use_preferred_lifetime = false;
            this.use_recycle_leases = false;
            this.use_update_dns_on_lease_renewal = false;
            this.use_valid_lifetime = false;
            this.valid_lifetime = 43200;
        }
    }
}
