using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs;
using BAMCIS.Infoblox.Common.InfobloxStructs.Discovery;
using BAMCIS.Infoblox.Common.InfobloxStructs.Grid.CloudApi;
using BAMCIS.Infoblox.Common.InfobloxStructs.Properties;
using System;
using System.Net;
using System.Net.Sockets;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    public class ipv6range : BaseNameCommentObject
    {
        private string _end_addr;
        private string _ipv6_end_prefix;
        private string _ipv6_start_prefix;
        private string _name;
        private string _network;
        private string _start_addr;

        [Searchable(Equality = true)]
        public IPv6AddressTypeEnum address_type { get; set; }
        public info cloud_info { get; set; }
        public bool disable { get; set; }
        public DiscoverNowStatusEnum discover_now_status { get; set;}
        public basicpollsettings discovery_basic_poll_settings { get; set; }
        public blackoutsetting discovery_blackout_setting { get; set; }
        public string discovery_member { get; set; }
        public bool enable_discovery { get; set; }
        public bool enable_immediate_discovery { get; set; }
        [Searchable(Equality = true, Regex = true)]
        public string end_addr
        {
            get
            {
                return this._end_addr;
            }
            set 
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    this._end_addr = value;
                }
                else
                {
                    throw new ArgumentException("The end address must be a valid IPv6 address.");
                }
            }
        }
        public exclusionrange[] exclude { get; set; }
        [Searchable(Equality = true, Regex = true)]
        public string ipv6_end_prefix
        {
            get
            {
                return this._ipv6_end_prefix;
            }
            set
            {
                if (NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._ipv6_end_prefix = value;
                }
                else
                {
                    throw new ArgumentException("The IPv6 end prefix must be a valid IPv6/CIDR format.");
                }
            }
        }
        [Searchable(Equality = true)]
        public uint ipv6_prefix_bits { get; set; }
        [Searchable(Equality = true, Regex = true)]
        public string ipv6_start_prefix
        {
            get
            {
                return this._ipv6_start_prefix;
            }
            set
            {
                if (NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._ipv6_start_prefix = value;
                }
                else
                {
                    throw new ArgumentException("The IPv6 start prefix must be a valid IPv6/CIDR format.");
                }
            }
        }
        [Searchable(Equality = true)]
        public dhcpmember member { get; set; }
        [Searchable(Equality = true, Regex = true)]
        public override string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.Trim();
            }
        }
        [Searchable(Equality = true, Regex = true)]
        public string network
        {
            get
            {
                return this._network;
            }
            set
            {
                if (NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new ArgumentException("The network property must be a valid IPv6/CIDR notation.");
                }
            }
        }
        [Searchable(Equality = true)]
        public string network_view { get; set; }
        public blackoutsetting port_control_blackout_setting { get; set; }
        public bool recycle_leases { get; set; }
        public bool restart_if_needed { get; set; }
        public bool same_port_control_discovery_blackout { get; set;}
        [Searchable(Equality = true)]
        public ServerAssociationTypeEnum server_association_type { get; set; }
        [Searchable(Equality = true, Regex = true)]
        public string start_addr
        {
            get
            {
                return this._start_addr;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    this._start_addr = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The start address must be a valid IPv6 address.");
                }
            }
        }
        public string template { get; set; }
        public bool use_blackout_setting { get; set; }
        public bool use_discovery_basic_polling_settings { get; set; }
        public bool use_enable_discovery { get; set; }
        public bool use_recycle_leases { get; set; }

        public ipv6range(string network)
        {
            this.address_type = IPv6AddressTypeEnum.ADDRESS;
            this.comment = String.Empty;
            this.disable = false;
            this.discovery_basic_poll_settings = new basicpollsettings()
            {
                auto_arp_refresh_before_switch_port_polling = true,
                complete_ping_sweep = false,
                device_profile = false,
                netbios_scanning = false,
                port_scanning = false,
                smart_subnet_ping_sweep = false,
                snmp_collection = true,
                switch_port_data_collection_polling = PollingModeEnum.PERIODIC,
                switch_port_data_collection_polling_interval = 3600
            };
            this.discovery_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.discovery_member = String.Empty;
            this.enable_discovery = false;
            this.exclude = new exclusionrange[0];
            this.name = String.Empty;
            this.network = network;
            this.network_view = "The default network view";
            this.port_control_blackout_setting = new blackoutsetting() { enable_blackout = false };
            this.recycle_leases = true;
            this.restart_if_needed = false;
            this.same_port_control_discovery_blackout = false;
            this.server_association_type = ServerAssociationTypeEnum.NONE;
            this.template = String.Empty;
            this.use_blackout_setting = false;
            this.use_discovery_basic_polling_settings = false;
            this.use_enable_discovery = false;
            this.use_recycle_leases = false;
        }
    }
}
