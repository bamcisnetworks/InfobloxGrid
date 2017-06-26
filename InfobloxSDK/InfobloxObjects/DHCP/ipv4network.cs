using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Setting.Ipam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("network")]
    public class ipv4network : BaseIPv4andIPv6Network
    {
        private string _bootserver;
        private List<string> _email_list;
        private uint _high_water_mark;
        private uint _high_water_mark_reset;
        private List<string> _ignore_mac_addresses;
        private List<string> _ipam_email_addresses;
        private string _ipv4addr;
        private int _lease_scavenge_time;
        private uint _low_water_mark;
        private uint _low_water_mark_reset;
        private List<object> _members;
        private uint _netmask;
        private string _network;
        private string _nextserver;

        public bool authority { get; set; }

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
                    else if (NetworkAddressTest.IsIPv4Address(value, out ip) || NetworkAddressTest.IsIPv6Address(value, out ip, false))
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

        public bool ddns_update_fixed_addresses { get; set; }

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

        public bool enable_dhcp_thresholds { get; set; }

        public bool enable_email_warnings { get; set; }

        public bool enable_snmp_warnings { get; set; }

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
                    throw new ArgumentException("The high water mark value must be between 1 and 100.");
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
                        throw new ArgumentException("The high water mark reset must be less than the high water mark or 0.");
                    }
                }
                else
                {
                    throw new ArgumentException("The high water mark reset value must be between 1 and 100.");
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

                foreach (string val in value)
                {
                    string mac;
                    if (NetworkAddressTest.IsMAC(val, out mac))
                    {
                        this._ignore_mac_addresses.Add(mac);
                    }
                    else
                    {
                        throw new ArgumentException("All values in ignore mac addresses must be valid MACs.");
                    }
                }
            }
        }

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
                    this._ipam_email_addresses.Add(email);
                }
            }
        }

        public threshold ipam_threshold_settings { get; set; }

        public trap ipam_trap_settings { get; set; }

        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                IPAddress ip;

                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    this._ipv4addr = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The ipv4addr property must be a valid IPv4 address.");
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
                if (value == -1 || value > 86400)
                {
                    this._lease_scavenge_time = value;
                }
                else
                {
                    throw new ArgumentException("The lease scavenge time must be -1 or greater than 86400.");
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
                if (value >=0 && value <= 100)
                {
                    this._low_water_mark = value;
                }
                else
                {
                    throw new ArgumentException("The low water mark value must be between 1 and 100.");
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
                        throw new ArgumentException("The low water mark reset must be greater than the low water mark or 0.");
                    }
                }
                else
                {
                    throw new ArgumentException("The low water mark reset value must be between 1 and 100.");
                }
            }
        }

        public override object[] members
        {
            get
            {
                return this._members.ToArray();
            }
            set
            {
                this._members = new List<object>();

                if (value.Length > 0)
                {
                    Type type = value[0].GetType();

                    if (value.ToList().Where(x => x.GetType().Equals(type)).Count().Equals(value.Length))
                    {
                        if (type.Equals(typeof(msdhcpserver)) || type.Equals(typeof(dhcpmember)))
                        {
                            this._members = new List<object>();

                            foreach (object val in value)
                            {
                                object newMember = Activator.CreateInstance(type);
                                foreach (PropertyInfo info in type.GetTypeInfo().GetProperties())
                                {
                                    info.SetValue(newMember, val.GetType().GetTypeInfo().GetProperty(info.Name).GetValue(val));
                                }
                                this._members.Add(newMember);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("The type of the array must be msdhcpserver or dhcpmember.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("All objects in the members property must be of the same type.");
                    }
                }
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
                if (value >=0 && value <= 32)
                {
                    this._netmask = value;
                }
                else
                {
                    throw new ArgumentException("The netmask must be between 0 and 32.");
                }
            }
        }

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
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
                    throw new ArgumentException("The network property must be a valid IPv4 CIDR address.");
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
                IPAddress ip;
                if (NetworkAddressTest.IsFqdn(value))
                {
                    this._nextserver = value;
                }
                else if (NetworkAddressTest.IsIPv4Address(value, out ip))
                {
                    this._nextserver = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The next server must be a valid FQDN or IPv4 address.");
                }
            }
        }

        public uint pxe_lease_time { get; set; }

        public bool use_authority { get; set; }

        public bool use_bootfile { get; set; }

        public bool use_bootserver { get; set; }

        public bool use_ddns_update_fixed_addresses { get; set; }

        public bool use_deny_bootp { get; set; }

        public bool use_email_list { get; set; }

        public bool use_enable_dhcp_thresholds { get; set; }

        public bool use_ignore_dhcp_option_list_request { get; set; }

        public bool use_ignore_id { get; set; }

        public bool use_ipam_email_addresses { get; set; }

        public bool use_ipam_threshold_settings { get; set; }

        public bool use_ipam_trap_settings { get; set; }

        public bool use_lease_scavenge_time { get; set; }

        public bool use_nextserver { get; set; }

        public ipv4network(string network)
        {
            this.authority = false;
            this.bootfile = String.Empty;
            this.ddns_update_fixed_addresses = false;
            this.ddns_use_option81 = false;
            this.deny_bootp = false;
            this.email_list = new string[0];
            this.enable_dhcp_thresholds = false;
            this.enable_discovery = false;
            this.enable_email_warnings = false;
            this.enable_snmp_warnings = false;
            this.high_water_mark = 95;
            this.ignore_id = IgnoreIdEnum.NONE;
            this.ignore_mac_addresses = new string[0];
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
            this.members = new object[0];
            this.network = network;
            this.use_authority = false;
            this.use_bootfile = false;
            this.use_bootserver = false;
            this.use_ddns_update_fixed_addresses = false;
            this.use_ddns_use_option81 = false;
            this.use_deny_bootp = false;
            this.use_email_list = false;
            this.use_enable_dhcp_thresholds = false;
            this.use_enable_discovery = false;
            this.use_ignore_dhcp_option_list_request = false;
            this.use_ignore_id = false;
            this.use_ipam_email_addresses = false;
            this.use_ipam_threshold_settings = false;
            this.use_ipam_trap_settings = false;
            this.use_lease_scavenge_time = false;
            this.use_nextserver = false;
        }
    }
}
