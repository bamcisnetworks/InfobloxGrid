using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs;
using BAMCIS.Infoblox.Common.InfobloxStructs.Discovery;
using BAMCIS.Infoblox.Common.InfobloxStructs.Grid.CloudApi;
using BAMCIS.Infoblox.Common.InfobloxStructs.Properties;

namespace BAMCIS.Infoblox.Common.BaseObjects
{
    public abstract class BaseNetworkContainer : BaseCommentObject
    {
        private string _network_view;

        [NotReadableAttribute]
        public bool auto_create_reversezone { get; set; }
        public info cloud_info { get; set; }
        [ReadOnlyAttribute]
        public DiscoverNowStatusEnum discover_now_status { get; internal protected set; }
        public basicpollsettings discovery_basic_poll_settings { get; set; }
        public blackoutsetting discovery_blackout_setting { get; set; }
        public string discovery_member { get; set; }
        [NotReadableAttribute]
        public bool enable_discovery { get; set; }
        public bool enable_immediate_discovery { get; set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string network_container { get; internal protected set; }
        [SearchableAttribute(Equality = true)]
        public string network_view
        {
            get
            {
                return this._network_view;
            }
            set
            {
                this._network_view = value.TrimValue();
            }
        }
        public blackoutsetting port_control_blackout_setting { get; set; }
        [NotReadableAttribute]
        public bool restart_if_needed { get; set; }
        public bool same_port_control_discovery_blackout { get; set; }
        [SearchableAttribute(Equality = true)]
        public bool unmanaged { get; set; }
        public bool use_blackout_setting { get; set; }
        public bool use_discovery_basic_polling_settings { get; set; }
        public bool use_enable_discovery { get; set; }
        public bool use_zone_associations { get; set; }
        public zoneassociation[] zone_associations { get; set; }

        public BaseNetworkContainer()
        {
            this.auto_create_reversezone = false;
            this.discovery_basic_poll_settings = new basicpollsettings() { auto_arp_refresh_before_switch_port_polling = true, complete_ping_sweep = false, device_profile = false, netbios_scanning = false, port_scanning = false, snmp_collection = true, switch_port_data_collection_polling = PollingModeEnum.PERIODIC, switch_port_data_collection_polling_interval = 3600 };
            this.discovery_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.enable_discovery = false;
            this.network_view = "default";
            this.port_control_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.restart_if_needed = false;
            this.same_port_control_discovery_blackout = false;
            this.unmanaged = false;
            this.use_blackout_setting = false;
            this.use_discovery_basic_polling_settings = false;
            this.use_enable_discovery = false;
            this.use_zone_associations = true;
            this.zone_associations = new zoneassociation[0];
        }
    }
}
