using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Discovery
{
    [Name("discovery:deviceinterface")]
    public class deviceinterface : RefObject
    {     
        private string _cap_if_admin_status_na_reason;
        private string _cap_if_description_na_reason;
        private string _cap_if_net_deprovisioning_ipv4_na_reason;
        private string _cap_if_net_deprovisioning_ipv6_na_reason;
        private string _cap_if_net_provisioning_ipv4_na_reason;
        private string _cap_if_net_provisioning_ipv6_na_reason;
        private string _cap_if_vlan_assignment_na_reason;
        private string _cap_if_voice_vlan_na_reason;
        private string _description;
        private DateTime _last_change;
        private string _mac;
        private string _type;

        [ReadOnlyAttribute]
        public UpDownEnum admin_status { get; internal protected set; }

        [ReadOnlyAttribute]
        public portconfigadminstatus admin_status_task_info { get; internal protected set; }

        [ReadOnlyAttribute]
        public bool cap_if_admin_status_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_if_admin_status_na_reason {
            get
            {
                return _cap_if_admin_status_na_reason;
            }

            internal protected set
            {
                this._cap_if_admin_status_na_reason = value.TrimValue();
            }
        }

        [ReadOnly]
        public bool cap_if_description_ind { get; internal protected set; }

        [ReadOnly]
        public string cap_if_description_na_reason
        {
            get
            {
                return _cap_if_description_na_reason;
            }
            internal protected set
            {
                this._cap_if_description_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool cap_if_net_deprovisioning_ipv4_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_if_net_deprovisioning_ipv4_na_reason 
        {
            get
            {
                return this._cap_if_net_deprovisioning_ipv4_na_reason;
            }
            internal protected set
            {
                this._cap_if_net_deprovisioning_ipv4_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool cap_if_net_deprovisioning_ipv6_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_if_net_deprovisioning_ipv6_na_reason 
        {
            get
            {
                return this._cap_if_net_deprovisioning_ipv6_na_reason;
            }
            internal protected set
            {
                this._cap_if_net_deprovisioning_ipv6_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool cap_if_net_provisioning_ipv4_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_if_net_provisioning_ipv4_na_reason 
        {
            get
            {
                return this._cap_if_net_provisioning_ipv4_na_reason;
            }
            internal protected set
            {
                this._cap_if_net_provisioning_ipv4_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool cap_if_net_provisioning_ipv6_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_if_net_provisioning_ipv6_na_reason 
        {
            get
            {
                return this._cap_if_net_provisioning_ipv6_na_reason;
            }
            internal protected set
            {
                this._cap_if_net_provisioning_ipv6_na_reason = value.TrimValue();
            }
        }

        [ReadOnly]
        public bool cap_if_vlan_assignment_ind { get; internal protected set; }

        [ReadOnly]
        public string cap_if_vlan_assignment_na_reason
        {
            get
            {
                return this._cap_if_vlan_assignment_na_reason;
            }
            set
            {
                this._cap_if_vlan_assignment_na_reason = value.TrimValue();
            }
        }

        [ReadOnly]
        public bool cap_if_voice_vlan_ind { get; internal protected set; }

        [ReadOnly]
        public string cap_if_voice_vlan_na_reason
        {
            get
            {
                return this._cap_if_voice_vlan_na_reason;
            }
            set
            {
                this._cap_if_voice_vlan_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string description
        {
            get
            {
                return this._description;
            }
            internal protected set
            {
                this._description = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public portconfigdescription description_task_info { get; internal protected set; }

        [ReadOnlyAttribute]
        public string device { get; internal protected set; }

        [ReadOnlyAttribute]
        public DuplexStateEnum duplex { get; internal protected set; }

        [ReadOnlyAttribute]
        public ifaddrinfo[] ifaddr_infos { get; internal protected set; }

        [ReadOnlyAttribute]
        public int index { get; internal protected set; }

        [ReadOnlyAttribute]
        public long last_change 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._last_change);
            }
            internal protected set
            {
                this._last_change = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [ReadOnlyAttribute]
        public bool link_aggregation { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string mac 
        {
            get
            {
                return this._mac;
            }
            internal protected set
            {
                NetworkAddressTest.IsMAC(value, out this._mac, true, true);
            }
        }

        [ReadOnly]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string name { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public UpDownEnum oper_status { get; internal protected set; }

        [ReadOnlyAttribute]
        public EnabledDisabledStatusEnum port_fast { get; internal protected set; }

        [ReadOnlyAttribute]
        public string reserved_object { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Negative = true, LessThan = true, Equality = true, GreaterThan = true)]
        public uint speed { get; internal protected set; }

        [ReadOnlyAttribute]
        public OnOffStatusEnum trunk_status { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string type
        {
            get
            {
                return this._type;
            }
            internal protected set
            {
                this._type = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public portconfigvlaninfo vlan_info_task_info { get; internal protected set; }

        [ReadOnlyAttribute]
        public vlaninfo[] vlan_infos { get; internal protected set; }
    }
}
