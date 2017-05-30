using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Setting.Ipam;
using System;
using System.Collections.Generic;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("networktemplate")]
    public class networktemplate : BaseNameCommentObject
    {
        private string _bootfile;
        private string _bootserver;
        private string _ddns_domainname;
        private List<string> _email_list;
        private uint _high_water_mark;
        private uint _high_water_mark_reset;
        private List<string> _ipam_email_addresses;
        private int _lease_scavenge_time;
        private uint _low_water_mark;
        private uint _low_water_mark_reset;
        private List<object> _members;
        private uint _netmask;
        private string _nextserver;

        public bool allow_any_netmask { get; set; }
        public bool authority { get; set; }
        public bool auto_create_reversezone { get; set; }
        public string bootfile
        {
            get
            {
                return this._bootfile;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    IPAddress ip;
                    if (NetworkAddressTest.IsIPv4Address(value, out ip))
                    {
                        this._bootfile = ip.ToString();
                    }
                    else if (NetworkAddressTest.IsFqdn(value))
                    {
                        this._bootfile = value;
                    }
                    else
                    {
                        throw new ArgumentException("The bootfile must be a valid IPv4 address or FQDN.");
                    }
                }
                else
                {
                    this._bootfile = String.Empty;
                }
            }
        }
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
                    if (NetworkAddressTest.IsIPv4Address(value, out ip))
                    {
                        this._bootserver = ip.ToString();
                    }
                    else if (NetworkAddressTest.IsFqdn(value))
                    {
                        this._bootserver = value;
                    }
                    else
                    {
                        throw new ArgumentException("The boot server must be a valid IPv4 address or FQDN.");
                    }
                }
                else
                {
                    this._bootserver = String.Empty;
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
                NetworkAddressTest.IsFqdn(value, "ddns_domainname", out this._ddns_domainname, true, true);
            }
        }
        public bool ddns_generate_hostname { get; set; }
        public bool ddns_server_always_updates { get; set; }
        public uint ddns_ttl { get; set; }
        public bool ddns_update_fixed_addresses { get; set; }
        public bool ddns_use_option81 { get; set; }
        public dhcpmember delegated_member { get; set; }
        public bool deny_bootp { get; set; }
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
                    NetworkAddressTest.IsValidEmail(email, out temp, false, true);
                    this._email_list.Add(temp);
                }
            }
        }
        public bool enable_ddns { get; set; }
        public bool enable_dhcp_thresholds { get; set; }
        public bool enable_email_warnings { get; set;}
        public bool enable_snmp_warnings { get; set; }
        public string[] fixed_address_templates { get; set; }
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
        public string[] ipam_email_addresses
        {
            get
            {
                return this._ipam_email_addresses.ToArray();
            }
            set
            {
                this._ipam_email_addresses = new List<string>();

                foreach (string email in value)
                {
                    string temp = String.Empty;
                    NetworkAddressTest.IsValidEmail(email, out temp, false, true);
                    this._ipam_email_addresses.Add(temp);
                }
            }
        }
        public threshold ipam_threshold_settings { get; set; }
        public trap ipam_trap_settings { get; set; }
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
        public virtual object[] members
        {
            get
            {
                return this._members.ToArray();
            }
            set
            {
                this._members = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(dhcpmember), typeof(msdhcpserver) }, value, out this._members);
            }
        }
        public uint netmask
        {
            get
            {
                return this._netmask;
            }
            set
            {
                if (value >= 0 && value <= 32)
                {
                    this._netmask = value;
                }
                else
                {
                    throw new ArgumentException("The netmask must be between 0 and 32.");
                }
            }
        }
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
        public dhcpoption[] options { get; set; }
        public uint pxe_lease_time { get; set; }
        public string[] range_templates { get; set; }
        public bool recycle_leases { get; set; }
        public bool update_dns_on_lease_renewal { get; set; }
        public bool use_authority { get; set; }
        public bool use_bootfile { get; set; }
        public bool use_bootserver { get; set; }
        public bool use_ddns_domainname { get; set; }
        public bool use_ddns_generate_hostname { get; set; }
        public bool use_ddns_ttl { get; set; }
        public bool use_ddns_update_fixed_addresses { get; set; }
        public bool use_ddns_use_option81 { get; set; }
        public bool use_deny_bootp { get; set; }
        public bool use_email_list { get; set; }
        public bool use_enable_ddns { get; set; }
        public bool use_enable_dhcp_thresholds { get; set; }
        public bool use_ignore_dhcp_option_list_request { get; set;}
        public bool use_ipam_email_addresses { get; set; }
        public bool use_ipam_threshold_settings { get; set; }
        public bool use_ipam_trap_settings { get; set; }
        public bool use_lease_scavenge_time { get; set; }
        public bool use_nextserver { get; set; }
        public bool use_options { get; set; }
        public bool use_recycle_leases { get; set; }
        public bool use_update_dns_on_lease_renewal { get; set; }

        public networktemplate(string name)
        {
            this.allow_any_netmask = false;
            this.authority = false;
            this.auto_create_reversezone = false;
            this.bootfile = String.Empty;
            this.bootserver = String.Empty;
            this.cloud_api_compatible = false;
            this.ddns_domainname = String.Empty;
            this.ddns_generate_hostname = false;
            this.ddns_server_always_updates = true;
            this.ddns_ttl = 0;
            this.ddns_update_fixed_addresses = false;
            this.ddns_use_option81 = false;
            this.deny_bootp = false;
            this.email_list = new string[0];
            this.enable_ddns = false;
            this.enable_dhcp_thresholds = false;
            this.enable_email_warnings = false;
            this.enable_snmp_warnings = false;
            this.high_water_mark = 95;
            this.high_water_mark_reset = 85;
            this.ignore_dhcp_option_list_request = false;
            this.ipam_email_addresses = new string[0];
            this.ipam_threshold_settings = new threshold()
            {
                reset_value = 85,
                trigger_value = 95
            };
            this.ipam_trap_settings = new trap()
            {
                enable_email_warnings = false,
                enable_snmp_warnings = true
            };
            this.lease_scavenge_time = -1;
            this.low_water_mark = 0;
            this.low_water_mark_reset = 10;
            this.name = name;
            this.nextserver = String.Empty;
            this.options = new dhcpoption[] { new dhcpoption() { name = "dhcp-lease-time", num = 51, use_option = false, value = "43200", vendor_class = "DHCP" } };
            this.range_templates = new string[0];
            this.recycle_leases = true;
            this.update_dns_on_lease_renewal = false;
            this.use_authority = false;
            this.use_bootfile = false;
            this.use_bootserver = false;
            this.use_ddns_domainname = false;
            this.use_ddns_generate_hostname = false;
            this.use_ddns_ttl = false;
            this.use_ddns_update_fixed_addresses = false;
            this.use_ddns_use_option81 = false;
            this.use_deny_bootp = false;
            this.use_email_list = false;
            this.use_enable_ddns = false;
            this.use_enable_dhcp_thresholds = false;
            this.use_ignore_dhcp_option_list_request = false;
            this.use_ipam_email_addresses = false;
            this.use_ipam_threshold_settings = false;
            this.use_ipam_trap_settings = false;
            this.use_lease_scavenge_time = false;
            this.use_nextserver = false;
            this.use_options = false;
            this.use_recycle_leases = false;
            this.use_update_dns_on_lease_renewal = false;
        }
    }
}
