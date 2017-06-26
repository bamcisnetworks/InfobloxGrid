using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("roaminghost")]
    public class roaminghost : BaseNameCommentObject
    {
        private string _bootserver;
        private string _ddns_domainname;
        private string _ddns_hostname;
        private string _dhcp_client_identifier;
        private string _ipv6_client_hostname;
        private string _ipv6_ddns_domainname;
        private string _ipv6_ddns_hostname;
        private string _ipv6_domain_name;
        private List<string> _ipv6_domain_name_servers;
        private string _ipv6_duid;
        private string _ipv6_match_option;
        private string _mac;
        private string _nextserver;

        [SearchableAttribute(Equality = true)]
        [Basic]
        public AddressTypeEnum address_type { get; set; }

        public string bootfile { get; set; }

        public string bootserver
        {
            get
            {
                return this._bootserver;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "bootserver", out this._bootserver, true, true);
            }
        }

        public bool client_identifier_prepend_zero { get; set; }

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public override string comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                if (value.TrimValue().Length <= 256)
                {
                    this._comment = value.TrimValue();
                }
                else
                {
                    throw new ArgumentException("The comment cannot exceed 256 characters.");
                }
            }
        }

        public string ddns_domainname
        {
            get
            {
                return this._ddns_domainname;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "ddns_domainname", out this._ddns_domainname, true, true);
            }
        }

        public string ddns_hostname
        {
            get
            {
                return this._ddns_hostname;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "ddns_hostname", out this._ddns_hostname, true, true);
            }
        }

        public bool deny_bootp { get; set; }

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string dhcp_client_identifier
        {
            get
            {
                return this._dhcp_client_identifier;
            }
            set
            {
                this._dhcp_client_identifier = value.TrimValue();
            }
        }

        public bool disable { get; set; }

        public bool enable_ddns { get; set; }

        public bool force_roaming_hostname { get; set; }

        public bool ignore_dhcp_option_list_request { get; set; }

        [ReadOnlyAttribute]
        public string ipv6_client_hostname
        {
            get
            {
                return this._ipv6_client_hostname;
            }
            internal protected set
            {
                this._ipv6_client_hostname = value.TrimValue();
            }
        }

        public string ipv6_ddns_domainname
        {
            get
            {
                return this._ipv6_ddns_domainname;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "ipv6_ddns_domainname", out this._ipv6_ddns_domainname, true, true);
            }
        }

        public string ipv6_ddns_hostname
        {
            get
            {
                return this._ipv6_ddns_hostname;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "ipv6_ddns_hostname", out this._ipv6_ddns_hostname, true, true);
            }
        }

        public string ipv6_domain_name
        {
            get
            {
                return this._ipv6_domain_name;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "ipv6_domain_name", out this._ipv6_domain_name, true, true);
            }
        }

        public string[] ipv6_domain_name_servers
        {
            get
            {
                return this._ipv6_domain_name_servers.ToArray();
            }
            set
            {
                this._ipv6_domain_name_servers = new List<string>();

                foreach (string item in value)
                {
                    IPAddress IP;
                    if (NetworkAddressTest.IsIPv6Address(item, out IP, false, true))
                    {
                        this._ipv6_domain_name_servers.Add(IP.ToString());
                    }
                }
            }
        }

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string ipv6_duid
        {
            get
            {
                return this._ipv6_duid;
            }
            set
            {
                NetworkAddressTest.IsIPv6DUID(value, out this._ipv6_duid, true, true);
            }
        }

        public bool ipv6_enable_ddns { get; set; }

        public bool ipv6_force_roaming_hostname { get; set; }

        [SearchableAttribute(Equality = true)]
        public string ipv6_match_option
        {
            get
            {
                return this._ipv6_match_option;
            }
            set
            {
                string[] matches = new string[] { "DUID" };
                if (!String.IsNullOrEmpty(value))
                {
                    if (matches.Contains(value.ToUpper()))
                    {
                        this._ipv6_match_option = value.ToUpper();
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("The ipv6_match_option must be valid, {0} was provided, it must be one of the following {1}.", value, String.Join(", ", matches)));
                    }
                }
                else
                {
                    this._ipv6_match_option = String.Empty;
                }
            }
        }

        public dhcpoption[] ipv6_options { get; set; }

        [NotReadableAttribute]
        public string ipv6_template { get; set; }

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string mac
        {
            get
            {
                return this._mac;
            }
            set
            {
                NetworkAddressTest.IsMAC(value, out this._mac, true, true);
            }
        }

        public MatchClientRoamingHostEnum match_client { get; set; }

        [SearchableAttribute(Equality = true)]
        [Basic]
        public string network_view { get; set; }

        public string nextserver
        {
            get
            {
                return this._nextserver;
            }
            set
            {
                NetworkAddressTest.IsIPv4AddressOrFQDN(value, "nextserver", out this._nextserver, true, true);
            }
        }

        public dhcpoption[] options { get; set; }

        public uint preferred_lifetime { get; set; }

        public uint pxe_lease_time { get; set; }

        [NotReadableAttribute]
        public string template { get; set; }

        public bool use_bootfile { get; set; }

        public bool use_bootserver { get; set; }

        public bool use_ddns_domainname { get; set; }

        public bool use_deny_bootp { get; set; }

        public bool use_enable_ddns { get; set; }

        public bool use_ignore_dhcp_option_list_request { get; set; }

        public bool use_ipv6_ddns_domainname { get; set; }

        public bool use_ipv6_domain_name { get; set; }

        public bool use_ipv6_domain_name_servers { get; set; }

        public bool use_ipv6_enable_ddns { get; set; }

        public bool use_ipv6_options { get; set; }

        public bool use_nextserver { get; set; }

        public bool use_options { get; set; }

        public bool use_preferred_lifetime { get; set; }

        public bool use_valid_lifetime { get; set; }

        public uint valid_lifetime { get; set; }


        public roaminghost()
        {
            this.address_type = AddressTypeEnum.IPV4;
            this.bootfile = String.Empty;
            this.bootserver = String.Empty;
            this.client_identifier_prepend_zero = false;
            this.ddns_domainname = String.Empty;
            this.ddns_hostname = String.Empty;
            this.deny_bootp = false;
            this.dhcp_client_identifier = String.Empty;
            this.disable = false;
            this.enable_ddns = false;
            this.force_roaming_hostname = false;
            this.ignore_dhcp_option_list_request = false;
            this.ipv6_ddns_domainname = String.Empty;
            this.ipv6_ddns_hostname = String.Empty;
            this.ipv6_domain_name = String.Empty;
            this.ipv6_domain_name_servers = new string[0];
            this.ipv6_duid = String.Empty;
            this.ipv6_enable_ddns = false;
            this.ipv6_force_roaming_hostname = false;
            this.ipv6_match_option = String.Empty;
            this.ipv6_options = dhcpoption.DefaultArray();
            this.ipv6_template = String.Empty;
            this.mac = String.Empty;
            this.network_view = "default";
            this.nextserver = String.Empty;
            this.options = dhcpoption.DefaultArray();
            this.preferred_lifetime = 27000;
            this.template = String.Empty;
            this.use_bootfile = false;
            this.use_bootserver = false;
            this.use_ddns_domainname = false;
            this.use_deny_bootp = false;
            this.use_enable_ddns = false;
            this.use_ignore_dhcp_option_list_request = false;
            this.use_ipv6_ddns_domainname = false;
            this.use_ipv6_domain_name = false;
            this.use_ipv6_domain_name_servers = false;
            this.use_ipv6_enable_ddns = false;
            this.use_ipv6_options = false;
            this.use_nextserver = false;
            this.use_options = false;
            this.use_preferred_lifetime = false;
            this.use_valid_lifetime = false;
            this.valid_lifetime = 43200;
        }
    }
}
