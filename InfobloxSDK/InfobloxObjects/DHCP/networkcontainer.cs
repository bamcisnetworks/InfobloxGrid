using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using BAMCIS.Infoblox.Common.InfobloxStructs.Setting.Ipam;
using System;
using System.Collections.Generic;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("networkcontainer")]
    public class networkcontainer : BaseNetworkContainer
    {
        private string _network;
        private List<string> _ipam_email_addresses;

        public string[] ipam_email_addresses
        {
            get
            {
                return this._ipam_email_addresses.ToArray();
            }
            set
            {
                if (value.Length > 0)
                {
                    this._ipam_email_addresses = new List<string>();

                    foreach (string email in value)
                    {
                        string temp = String.Empty;
                        NetworkAddressTest.IsValidEmailWithException(email, out temp);
                        this._ipam_email_addresses.Add(temp);
                    }
                }
            }
        }
        public threshold ipam_threshold_settings { get; set; }
        public trap ipam_trap_settings { get; set; }
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
                if (NetworkAddressTest.IsIPv4Cidr(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new ArgumentException("The network must be in IPv4/CIDR format.");
                }
            }
        }
        public bool use_ipam_email_addresses { get; set; }
        public bool use_ipam_threshold_settings { get; set; }
        public bool use_ipam_trap_settings { get; set; }

        public networkcontainer(string network)
        {
            this.ipam_email_addresses = new string[0];
            this.ipam_threshold_settings = new threshold() { reset_value = 85, trigger_value = 95 };
            this.ipam_trap_settings = new trap() { enable_email_warnings = false, enable_snmp_warnings = true };
            this.network = network;
            this.use_ipam_email_addresses = false;
            this.use_ipam_threshold_settings = false;
            this.use_ipam_trap_settings = false;
        }
    }
}
