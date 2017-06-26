using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using System;
using System.Collections.Generic;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("member:dhcpproperties")]
    public class dhcpproperties
    {
        private string _host_name;
        private List<string> _ignore_mac_addresses;
        private string _ipv4addr;
        private string _ipv6addr;

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string host_name
        {
            get
            {
                return this._host_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "host_name", out this._host_name, true, true);
            }
        }

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
                    NetworkAddressTest.IsMAC(item, out temp, false, true);
                    this._ignore_mac_addresses.Add(temp);
                }
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            internal protected set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv4Address(value, out IP, true, true))
                {
                    this._ipv4addr = IP.ToString();
                }
            }
        }

        [ReadOnlyAttribute]
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
                IPAddress IP;
                if (NetworkAddressTest.IsIPv6Address(value, out IP, true, true))
                {
                    this._ipv6addr = IP.ToString();
                }
            }
        }

        public dhcpoption[] options { get; set; }

        public bool use_ignore_id { get; set; }

        public bool use_options { get; set; }

        public dhcpproperties()
        {
            this.ignore_id = IgnoreIdEnum.NONE;
            this.ignore_mac_addresses = new string[0];
            this.options = dhcpoption.DefaultArray();
            this.use_ignore_id = false;
            this.use_options = false;
        }
    }
}
