using BAMCIS.Infoblox.Common.Enums;
using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class dhcpddns
    {
        private string _dns_ext_primary;

        public string dns_ext_primary
        {
            get
            {
                return this._dns_ext_primary;
            }
            set
            {
                if (NetworkAddressTest.IsFqdn(value))
                {
                    this._dns_ext_primary = value;
                }
                else
                {
                    throw new ArgumentException("Dns external primary must be a valid FQDN.");
                }
            }
        }

        public string dns_grid_primary { get; set; }

        public string dns_grid_zone { get; set; }

        public ZoneMatchEnum zone_match { get; set; }
    }
}
