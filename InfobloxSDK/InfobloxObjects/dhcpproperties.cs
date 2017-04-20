using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs;
using System;
using System.Collections.Generic;

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
        public string host_name
        {
            get
            {
                return this._host_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdnWithExceptionAllowEmpty(value, "host_name", out this._host_name);
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
                    NetworkAddressTest.IsMACWithException(item, out temp);
                    this._ignore_mac_addresses.Add(temp);
                }
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            internal protected set
            {
                NetworkAddressTest.isIPv4WithExceptionAllowEmpty(value, out this._ipv4addr);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv6addr
        {
            get
            {
                return this._ipv6addr;
            }
            set
            {
                NetworkAddressTest.isIPv6WithExceptionAllowEmpty(value, out this._ipv6addr);
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
