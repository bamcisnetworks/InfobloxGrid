using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;
using BAMCIS.Infoblox.Core.InfobloxStructs.Properties;
using System;
using System.Collections.Generic;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("range")]
    public class ipv4range : BaseNameCommentObject
    {
        private string _bootserver;
        private string _ddns_domainname;
        private List<string> _email_list;
        private string _end_addr;
        private uint _high_water_mark;
        private uint _high_water_mark_reset;
        private List<string> _ignore_mac_addresses;
        private string _known_clients;
        private int _lease_scavenge_time;
        private uint _low_water_mark;
        private uint _low_water_mark_reset;
        private string _network;
        private string _nextserver;
        private uint _split_scope_exclusion_percent;
        private string _start_addr;
        private string _unknown_clients;

        public bool always_update_dns { get; set; }
        public string bootfile { get; set; }
        public string bootserver
        {
            get
            {
                return this._bootserver;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    IPAddress ip;

                    if (NetworkAddressTest.IsFqdn(value))
                    {
                        this._bootserver = value;
                    }
                    else if (NetworkAddressTest.IsIPv4Address(value, out ip) || NetworkAddressTest.IsIPv6Address(value, out ip))
                    {
                        this._bootserver = ip.ToString();
                    }
                    else
                    {
                        throw new ArgumentException("The boot server must be a valid FQDN or IP.");
                    }
                }
                else
                {
                    this._bootserver = String.Empty;
                }
            }
        }
        public info cloud_info { get; set; }
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
        public bool ddns_generate_hostname { get; set; }
        public bool deny_all_clients { get; set; }
        public bool deny_bootp { get; set; }
        public bool disable { get; set; }
        [ReadOnlyAttribute]
        public DiscoverNowStatusEnum discover_now_status { get; internal protected set; }
        public basicpollsettings discovery_basic_poll_settings { get; set; }
        public blackoutsetting discovery_blackout_setting { get; set; }
        public string discovery_member { get; set; }
        public string[] email_list
        {
            get
            {
                return this._email_list.ToArray();
            }
            set
            {
                this._email_list = new List<string>();
                foreach (string email in value)
                {
                    string temp = String.Empty;
                    if (NetworkAddressTest.IsValidEmail(email, out temp, false, true))
                    {
                        this._email_list.Add(email);
                    }
                }
            }
        }
        public bool enable_ddns { get; set; }
        public bool enable_dhcp_thresholds { get; set; }
        public bool enable_discovery { get; set; }
        public bool enable_email_warnings { get; set; }
        public bool enable_ifmap_publishing { get; set; }
        [NotReadableAttribute]
        public bool enable_immediate_discovery { get; set; }
        public bool enable_snmp_warnings { get; set; }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string end_addr
        {
            get
            {
                return this._end_addr;
            }
            set
            {
                IPAddress ip;
                if (NetworkAddressTest.IsIPv4Address(value, out ip))
                {
                    this._end_addr = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The end address must be a valid IPv4 address.");
                }
            }
        }
        public exclusionrange[] exclude { get; set; }
        [SearchableAttribute(Equality = true)]
        public string failover_association { get; set; }
        public filterrule[] fingerprint_filter_rules { get; set; }
        public uint high_water_mark
        {
            get
            {
                return this._high_water_mark;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    this._high_water_mark = value;
                }
                else
                {
                    throw new ArgumentException("The high water mark must be between 0 and 100.");
                }
            }
        }
        public uint high_water_mark_reset
        {
            get
            {
                return this._high_water_mark_reset;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    if (value < this.high_water_mark || value == 0)
                    {
                        this._high_water_mark_reset = value;
                    }
                    else
                    {
                        throw new ArgumentException("The high water mark reset must be lower than the high water mark.");
                    }
                }
                else
                {
                    throw new ArgumentException("The high water mark reset must be between 0 and 100.");
                }
            }
        }
        public bool ignore_dhcp_option_list_request { get; set; }
        public IgnoreIdEnum ignore_id { get; set; }
        public string[] ignore_mac_addresses
        {
            get
            {
                return this._ignore_mac_addresses.ToArray();
            }
            set
            {
                this._ignore_mac_addresses = new List<string>();

                foreach(string mac in value)
                {
                    string temp = String.Empty;
                    NetworkAddressTest.IsMAC(mac, out temp, false, true);
                    this._ignore_mac_addresses.Add(temp);
                }
            }
        }
        [ReadOnlyAttribute]
        public bool is_split_scope { get; internal protected set; }
        public string known_clients
        {
            get
            {
                return this._known_clients;
            }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Equals("Allow") || value.Equals("Deny"))
                {
                    this._known_clients = value;
                }
                else
                {
                    throw new ArgumentException("The known clients value must be Allow or Deny or String.Empty.");
                }
            }
        }
        public int lease_scavenge_time
        {
            get
            {
                return this._lease_scavenge_time;
            }
            set
            {
                if (value >= 86400 || value == -1)
                {
                    this._lease_scavenge_time = value;
                }
                else
                {
                    throw new ArgumentException("The lease scavenge time must be greater than or equal to 86400 or -1.");
                }
            }
        }
        public logicfilterrule[] logic_filter_rules { get; set; }
        public uint low_water_mark
        {
            get
            {
                return this._low_water_mark;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    this._low_water_mark = value;
                }
                else
                {
                    throw new ArgumentException("The low water mark must be between 0 and 100.");
                }
            }
        }
        public uint low_water_mark_reset
        {
            get
            {
                return this._low_water_mark_reset;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    if (value > this.low_water_mark || value == 0)
                    {
                        this._low_water_mark_reset = value;
                    }
                    else
                    {
                        throw new ArgumentException("The low water mark reset must be greater than the low watermark value.");
                    }
                }
                else
                {
                    throw new ArgumentException("The low water mark reset must be between 0 and 100.");
                }
            }
        }
        public filterrule[] mac_filter_rules { get; set; }
        [SearchableAttribute(Equality = true)]
        public dhcpmember member { get; set; }
        public msdhcpoption[] ms_options { get; set; }
        [SearchableAttribute(Equality = true)]
        public msdhcpserver ms_server { get; set; }
        public filterrule[] nac_filter_rules { get; set; }
        [SearchableAttribute(Equality = true, Regex = true)]
        public string network
        {
            get
            {
                return this._network;
            }
            set
            {
                if (NetworkAddressTest.IsIPv4Cidr(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new ArgumentException("The network must be a valid IPv4/CIDR format.");
                }
            }
        }
        [SearchableAttribute(Equality = true)]
        public string network_view { get; set; }
        public string nextserver
        {
            get
            {
                return this._nextserver;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    IPAddress ip;
                    if (NetworkAddressTest.IsIPv4Address(value, out ip))
                    {
                        this._nextserver = ip.ToString();
                    }
                    else if (NetworkAddressTest.IsFqdn(value))
                    {
                        this._nextserver = value;
                    }
                    else
                    {
                        throw new ArgumentException("The next server value must be a valid IPv4 address of FQDN.");
                    }
                }
                else
                {
                    this._nextserver = String.Empty;
                }
            }
        }
        public filterrule[] option_filter_rules { get; set; }
        public dhcpoption[] options { get; set; }
        public blackoutsetting port_control_blackout_setting { get; set; }
        public uint pxe_lease_time { get; set; }
        public bool recycle_leases { get; set; }
        public filterrule[] relay_agent_filter_rules { get; set; }
        [NotReadableAttribute]
        public bool restart_if_needed { get; set; }
        public bool same_port_control_discovery_blackout { get; set; }
        [SearchableAttribute(Equality = true)]
        public ServerAssociationTypeExpandedEnum server_association_type { get; set; }
        [NotReadableAttribute]
        public uint split_scope_exclusion_percent
        {
            get
            {
                return this._split_scope_exclusion_percent;
            }
            set
            {
                if (value >= 1 && value <= 99)
                {
                    this._split_scope_exclusion_percent = value;
                }
                else
                {
                    throw new ArgumentException("The value for the split scope percentage must be between 1 and 99.");
                }
            }
        }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string start_addr
        {
            get
            {
                return this._start_addr;
            }
            set
            {
                IPAddress ip;
                if (NetworkAddressTest.IsIPv4Address(value, out ip))
                {
                    this._start_addr = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The start address must be a valid IPv4 address.");
                }
            }
        }
        [NotReadableAttribute]
        public string template { get; set; }
        public string unknown_clients
        {
            get
            {
                return this._unknown_clients;
            }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Equals("Allow") || value.Equals("Deny"))
                {
                    this._unknown_clients = value;
                }
                else
                {
                    throw new ArgumentException("The unknown clients value must be Allow or Deny or String.Empty.");
                }
            }
        }
        public bool update_dns_on_lease_renewal { get; set; }
        public bool use_blackout_setting { get; set; }
        public bool use_bootfile { get; set; }
        public bool use_bootserver { get; set; }
        public bool use_ddns_domainname { get; set; }
        public bool use_ddns_generate_hostname { get; set; }
        public bool use_deny_bootp { get; set; }
        public bool use_discovery_basic_poll_settings { get; set; }
        public bool use_email_list { get; set; }
        public bool use_enable_ddns { get; set; }
        public bool use_enable_dhcp_thresholds { get; set; }
        public bool use_enable_discovery { get; set; }
        public bool use_enable_ifmap_publishing { get; set; }
        public bool use_ignore_dhcp_option_list_request { get; set; }
        public bool use_ignore_id { get; set; }
        public bool use_known_clients { get; set; }
        public bool use_lease_scavenge_time { get; set; }
        public bool use_nextserver { get; set; }
        public bool use_options { get; set; }
        public bool use_recycle_leases { get; set; }
        public bool use_unknown_clients { get; set; }
        public bool use_update_dns_on_lease_renewal { get; set; }

        public ipv4range(string end_addr, string start_addr)
        {
            this.always_update_dns = false;
            this.bootfile = String.Empty;
            this.ddns_domainname = String.Empty;
            this.ddns_generate_hostname = false;
            this.deny_all_clients = false;
            this.deny_bootp = false;
            this.disable = false;
            this.discovery_basic_poll_settings = new basicpollsettings() { auto_arp_refresh_before_switch_port_polling = true, complete_ping_sweep = false, device_profile = false, netbios_scanning = false, port_scanning = false, snmp_collection = true, switch_port_data_collection_polling = PollingModeEnum.PERIODIC, switch_port_data_collection_polling_interval = 3600 };
            this.discovery_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.discovery_member = String.Empty;
            this.email_list = new string[0];
            this.enable_ddns = false;
            this.enable_dhcp_thresholds = false;
            this.enable_discovery = false;
            this.enable_email_warnings = false;
            this.enable_ifmap_publishing = false;
            this.enable_snmp_warnings = false;
            this.end_addr = end_addr;
            this.exclude = new exclusionrange[0];
            this.failover_association = String.Empty;
            this.fingerprint_filter_rules = new filterrule[0];
            this.high_water_mark = 95;
            this.high_water_mark_reset = 85;
            this.ignore_dhcp_option_list_request = false;
            this.ignore_id = IgnoreIdEnum.NONE;
            this.ignore_mac_addresses = new string[0];
            this.known_clients = String.Empty;
            this.lease_scavenge_time = -1;
            this.logic_filter_rules = new logicfilterrule[0];
            this.low_water_mark = 0;
            this.low_water_mark_reset = 10;
            this.mac_filter_rules = new filterrule[0];
            this.ms_options = new msdhcpoption[0];
            this.nac_filter_rules = new filterrule[0];
            this.network_view = "default";
            this.nextserver = String.Empty;
            this.option_filter_rules = new filterrule[0];
            this.options = new dhcpoption[] { new dhcpoption() { name = "dhcp-lease-time", num = 51, use_option = false, value = "43200", vendor_class = "DHCP" } };
            this.port_control_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.recycle_leases = true;
            this.relay_agent_filter_rules = new filterrule[0];
            this.restart_if_needed = false;
            this.same_port_control_discovery_blackout = false;
            this.server_association_type = ServerAssociationTypeExpandedEnum.NONE;
            this.start_addr = start_addr;
            this.template = String.Empty;
            this.unknown_clients = String.Empty;
            this.update_dns_on_lease_renewal = false;
            this.use_blackout_setting = false;
            this.use_bootfile = false;
            this.use_bootserver = false;
            this.use_ddns_domainname = false;
            this.use_ddns_generate_hostname = false;
            this.use_deny_bootp = false;
            this.use_discovery_basic_poll_settings = false;
            this.use_email_list = false;
            this.use_enable_ddns = false;
            this.use_enable_dhcp_thresholds = false;
            this.use_enable_discovery = false;
            this.use_enable_ifmap_publishing = false;
            this.use_ignore_dhcp_option_list_request = false;
            this.use_ignore_id = false;
            this.use_known_clients = false;
            this.use_lease_scavenge_time = false;
            this.use_nextserver = false;
            this.use_options = false;
            this.use_recycle_leases = false;
            this.use_unknown_clients = false;
            this.use_update_dns_on_lease_renewal = false;

        }
    }
}
