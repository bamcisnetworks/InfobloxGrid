using BAMCIS.Infoblox.Common.Enums;
using System;
using System.Collections.Generic;

namespace BAMCIS.Infoblox.Common.BaseObjects
{
    //Properties shared by the network object and the shared network object
    public abstract class BaseNetwork : BaseNetworkLow
    {
        private string _bootserver;
        private List<string> _ignore_mac_addresses;
        private int _lease_scavenge_time;
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
                NetworkAddressTest.IsIPv4OrFqdnWithExceptionAllowEmpty(value, "bootserver", out this._bootserver);
            }
        }
        public bool ddns_update_fixed_addresses { get; set; }
        public bool ddns_use_option81 { get; set; }
        public bool deny_bootp { get; set; }
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

                foreach (string item in value)
                {
                    string temp = String.Empty;
                    NetworkAddressTest.IsMACWithException(item, out temp);
                    this._ignore_mac_addresses.Add(temp);
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
                if (value == -1 || value >= 86400)
                {
                    this._lease_scavenge_time = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("The lease scavenge time must be -1 or greater than or equal to 86400, {0} was provided.", value));
                }
            }
        }
        [SearchableAttribute(Equality = true)]
        public string nextserver
        {
            get
            {
                return this._nextserver;
            }
            set
            {
                NetworkAddressTest.IsIPv4OrFqdnWithExceptionAllowEmpty(value, "nextserver", out this._nextserver);
            }
        }
        public uint pxe_lease_time { get; set; }
        public bool use_authority { get; set; }
        public bool use_bootfile { get; set; }
        public bool use_bootserver { get; set; }
        public bool use_ddns_update_fixed_addresses { get; set; }
        public bool use_ddns_use_option81 { get; set; }
        public bool use_deny_bootp { get; set; }
        public bool use_ignore_dhcp_option_list_request { get; set; }
        public bool use_ignore_id { get; set; }
        public bool use_lease_scavenge_time { get; set; }
        public bool use_nextserver { get; set; }
        
        public BaseNetwork()
        {
            this.authority = false;
            this.bootfile = String.Empty;
            this.bootserver = String.Empty;
            this.ddns_update_fixed_addresses = false;
            this.ddns_use_option81 = false;
            this.deny_bootp = false;
            this.ignore_dhcp_option_list_request = false;
            this.ignore_id = IgnoreIdEnum.NONE;
            this.ignore_mac_addresses = new string[0];
            this.lease_scavenge_time = -1;
            this.nextserver = String.Empty;
            this.use_authority = false;
            this.use_bootfile = false;
            this.use_bootserver = false;
            this.use_ddns_update_fixed_addresses = false;
            this.use_ddns_use_option81 = false;
            this.use_deny_bootp = false;
            this.use_ignore_dhcp_option_list_request = false;
            this.use_ignore_id = false;
            this.use_lease_scavenge_time = false;
            this.use_nextserver = false;
        }
    }
}
