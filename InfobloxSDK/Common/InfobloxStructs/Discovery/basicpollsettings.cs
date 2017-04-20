using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs.Setting;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Discovery
{
    public class basicpollsettings
    {
        public bool auto_arp_refresh_before_switch_port_polling { get; set; }
        public bool complete_ping_sweep { get; set; }
        public bool device_profile { get; set; }
        public bool netbios_scanning { get; set; }
        public bool port_scanning { get; set; }
        public bool smart_subnet_ping_sweep { get; set; }
        public bool snmp_collection { get; set; }
        public PollingModeEnum switch_port_data_collection_polling { get; set; }
        public uint switch_port_data_collection_polling_interval { get; set; }
        public schedule switch_port_data_collection_polling_schedule { get; set; }
    }
}
