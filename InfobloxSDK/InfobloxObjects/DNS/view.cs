using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;
using System;
using System.Collections.Generic;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("view")]
    public class view : BaseNameCommentObject
    {
        private List<string> _blacklist_redirect_addresses;
        private List<string> _forwarders;
        private List<object> _match_clients;
        private List<object> _match_destinations;
        private List<string> _nxdomain_redirect_addresses;

        [SearchableAttribute(Equality = true)]
        public BlacklistActionEnum blacklist_action { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool blacklist_log_query { get; set; }

        public string[] blacklist_redirect_addresses
        {
            get
            {
                return this._blacklist_redirect_addresses.ToArray();
            }
            set
            {
                this._blacklist_redirect_addresses = new List<string>();

                foreach (string item in value)
                {
                    IPAddress IP;
                    if (NetworkAddressTest.IsIPv4Address(item, out IP, false, true))
                    {
                        this._blacklist_redirect_addresses.Add(IP.ToString());
                    }
                }
            }
        }

        public uint blacklist_redirect_ttl { get; set; }

        public string[] blacklist_rulesets { get; set; }

        [ReadOnlyAttribute]
        public info cloud_info { get; internal protected set; }

        public extserver[] custom_root_name_servers { get; set; }

        public bool disable { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool dns64_enabled { get; set; }

        public string[] dns64_groups { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool dnssec_enabled { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool dnssec_expired_signatures_enabled { get; set; }

        public string[] dnssec_negative_trust_anchors { get; set; }

        public dnssectrustedkey[] dnssec_trusted_keys { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool dnssec_validation_enabled { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool enable_blacklist { get; set; }

        [SearchableAttribute(Equality = true)]
        public FilterAAAAEnum filter_aaaa { get; set; }

        public addressac[] filter_aaaa_list { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool forward_only { get; set; }

        public string[] forwarders
        {
            get
            {
                return this._forwarders.ToArray();
            }
            set
            {
                this._forwarders = new List<string>();

                foreach (string item in value)
                {
                    IPAddress IP;
                    if (NetworkAddressTest.IsIPv4Address(item, out IP, false, true))
                    {
                        this._forwarders.Add(IP.ToString());
                    }
                }
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public bool is_default { get; internal protected set; }

        public uint lame_ttl { get; set; }

        public object[] match_clients
        {
            get
            {
                return this._match_clients.ToArray();
            }
            set
            {
                this._match_clients = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(addressac), typeof(tsigac) }, value, out this._match_clients);
            }
        }

        public object[] match_destinations
        {
            get
            {
                return this._match_destinations.ToArray();
            }
            set
            {
                this._match_destinations = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(addressac), typeof(tsigac) }, value, out this._match_destinations);
            }
        }

        public uint max_cache_ttl { get; set; }

        public uint max_ncache_ttl { get; set; }

        [SearchableAttribute(Equality = true)]
        public string network_view { get; set; }

        public uint notify_delay { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool nxdomain_log_query { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool nxdomain_redirect { get; set; }

        public string[] nxdomain_redirect_addresses
        {
            get
            {
                return this._nxdomain_redirect_addresses.ToArray();
            }
            set
            {
                this._nxdomain_redirect_addresses = new List<string>();

                foreach (string item in value)
                {
                    string temp = String.Empty;
                    NetworkAddressTest.IsIPAddress(item, out temp, false, true);
                    this._nxdomain_redirect_addresses.Add(temp);
                }
            }
        }

        public uint nxdomain_redirect_ttl { get; set; }

        public string[] nxdomain_rulesets { get; set; }

        [SearchableAttribute(Equality = true)]
        public bool recursion { get; set; }

        [SearchableAttribute(Equality = true)]
        public RootNameServerTypeEnum root_name_server_type { get; set; }

        public sortlist[] sortlist { get; set; }

        public bool use_blacklist { get; set; }

        public bool use_dns64 { get; set; }

        public bool use_dnssec { get; set; }

        public bool use_filter_aaaa { get; set; }

        public bool use_forwarders { get; set; }

        public bool use_lame_ttl { get; set; }

        public bool use_max_cache_ttl { get; set; }

        public bool use_max_ncache_ttl { get; set; }

        public bool use_nxdomain_redirect { get; set; }

        public bool use_recursion { get; set; }

        public bool use_root_name_server { get; set; }

        public bool use_sortlist { get; set; }

        public view()
        {
            this.blacklist_action = BlacklistActionEnum.REDIRECT;
            this.blacklist_log_query = false;
            this.blacklist_redirect_addresses = new string[0];
            this.blacklist_redirect_ttl = 60;
            this.blacklist_rulesets = new string[0];
            this.custom_root_name_servers = new extserver[0];
            this.disable = false;
            this.dns64_enabled = false;
            this.dns64_groups = new string[0];
            this.dnssec_enabled = false;
            this.dnssec_expired_signatures_enabled = false;
            this.dnssec_negative_trust_anchors = new string[0];
            this.dnssec_trusted_keys = new dnssectrustedkey[0];
            this.dnssec_validation_enabled = true;
            this.enable_blacklist = false;
            this.filter_aaaa = FilterAAAAEnum.NO;
            this.filter_aaaa_list = new addressac[0];
            this.forward_only = false;
            this.forwarders = new string[0];
            this.lame_ttl = 600;
            this.max_cache_ttl = 604800;
            this.max_ncache_ttl = 10800;
            this.network_view = "default";
            this.notify_delay = 5;
            this.nxdomain_log_query = false;
            this.nxdomain_redirect = false;
            this.nxdomain_redirect_addresses = new string[0];
            this.nxdomain_redirect_ttl = 60;
            this.nxdomain_rulesets = new string[0];
            this.recursion = false;
            this.root_name_server_type = RootNameServerTypeEnum.INTERNET;
            this.sortlist = new sortlist[0];
            this.use_blacklist = false;
            this.use_dns64 = false;
            this.use_dnssec = false;
            this.use_filter_aaaa = false;
            this.use_forwarders = false;
            this.use_lame_ttl = false;
            this.use_max_cache_ttl = false;
            this.use_max_ncache_ttl = false;
            this.use_nxdomain_redirect = false;
            this.use_recursion = false;
            this.use_root_name_server = false;
            this.use_sortlist = false;
        }
    }
}
