using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;

namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseHostRecord : RefObject
    {
        private string _host;

        public bool configure_for_dhcp { get; set; }

        [ReadOnlyAttribute]
        public DiscoverNowStatusEnum discover_now_status { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(ContainsSearchable = true)]
        public discoverydata discovered_data { get; internal protected set; }

        [ReadOnlyAttribute]
        public string host
        {
            get
            {
                return this._host;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "host", out this._host, false, true);
            }
        }

        public dhcpoption[] options { get; set; }

        public string reserved_interface { get; set; }

        public bool use_for_ea_inheritance { get; set; }

        public bool use_options { get; set; }

        public BaseHostRecord()
        {
            this.configure_for_dhcp = false;
            this.options = dhcpoption.DefaultArray();
            this.use_for_ea_inheritance = false;
            this.use_options = false;
        }
    }
}
