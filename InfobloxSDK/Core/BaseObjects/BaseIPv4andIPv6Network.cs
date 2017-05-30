using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;
using BAMCIS.Infoblox.Core.InfobloxStructs.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseIPv4andIPv6Network : BaseNetworkMid
    {
        private List<object> _members;

        [NotReadableAttribute]
        public bool auto_create_reversezone { get; set; }
        public info cloud_info { get; set; }
        [ReadOnlyAttribute]
        public DiscoverNowStatusEnum discover_now_status { get; internal protected set; }
        public basicpollsettings discovery_basic_poll_settings { get; set; }
        public blackoutsetting discovery_blackout_setting { get; set; }
        public string discovery_member { get; set; }
        public bool enable_discovery { get; set; }
        public bool enable_ifmap_publishing { get; set; }
        [NotReadableAttribute]
        public bool enable_immediate_discovery { get; set; }
        public virtual object[] members
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
                        if (type.Equals(typeof(dhcpmember)))
                        {
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
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string network_container { get; internal protected set; }
        public blackoutsetting port_control_blackout_setting { get; set; }
        public bool recycle_leases { get; set; }
        [NotReadableAttribute]
        public bool restart_if_needed { get; set; }
        public bool same_port_control_discovery_blackout { get; set; }
        [NotReadableAttribute]
        public string template { get; set; }
        [SearchableAttribute(Equality = true)]
        public bool unmanaged { get; set; }
        public bool use_blackout_setting { get; set; }
        public bool use_discovery_basic_polling_settings { get; set; }
        public bool use_enable_discovery { get; set; }
        public bool use_enable_ifmap_publishing { get; set; }
        public bool use_recycle_leases { get; set; }
        public bool use_zone_associations { get; set; }
        public zoneassociation[] zone_associations { get; set; }

        public BaseIPv4andIPv6Network()
        {
            this.auto_create_reversezone = false;
            this.discovery_basic_poll_settings = new basicpollsettings() { auto_arp_refresh_before_switch_port_polling = true, complete_ping_sweep = false, device_profile = false, netbios_scanning = false, port_scanning = false, snmp_collection = true, switch_port_data_collection_polling = PollingModeEnum.PERIODIC, switch_port_data_collection_polling_interval = 3600 };
            this.discovery_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.enable_discovery = false;
            this.enable_ifmap_publishing = false;
            this.port_control_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.recycle_leases = true;
            this.restart_if_needed = false;
            this.same_port_control_discovery_blackout = false;
            this.template = String.Empty;
            this.unmanaged = false;
            this.use_blackout_setting = false;
            this.use_discovery_basic_polling_settings = false;
            this.use_enable_discovery = false;
            this.use_enable_ifmap_publishing = false;
            this.use_recycle_leases = false;
            this.use_zone_associations = true;
            this.zone_associations = new zoneassociation[0];
        }
    }
}
