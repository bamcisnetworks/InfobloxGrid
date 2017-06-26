using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using System;
using System.Collections.Generic;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid
{
    [Name("grid:dhcpproperties")]
    public class dhcpproperties : RefObject
    {
        private List<string> _ignore_mac_addresses;

        [Basic]
        public bool disable_all_nac_filters { get; set; }

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

        public dhcpoption[] options { get; set; }

        public dhcpproperties()
        {
            this.disable_all_nac_filters = false;
            this.ignore_id = IgnoreIdEnum.NONE;
            this.ignore_mac_addresses = new string[0];
            this.options = dhcpoption.DefaultArray();
        }
    }
}
