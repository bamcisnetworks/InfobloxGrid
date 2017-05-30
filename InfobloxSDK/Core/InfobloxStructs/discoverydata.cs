using BAMCIS.Infoblox.Core.Enums;
using System;
using System.ComponentModel;
using System.Net;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    [Description("discovery_data")]
    public class discoverydata
    {
        private DateTime _first_discovered;
        private DateTime _last_discovered;
        private string _mac;
        private string _mgmt_ip_address;
        private string _netbios_name;
        private string _network_component_ip;

        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string device_model { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string device_port_name { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string device_port_type { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string device_type { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string device_vendor { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string discovered_name { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string discoverer { get; set; }
        public string duid { get; set; }
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true, Negative = true)]
        public long first_discovered 
        { 
            get
            {
                return UnixTimeHelper.ToUnixTime(this._first_discovered);
            }
            set
            {
                this._first_discovered = UnixTimeHelper.FromUnixTime(value);
            } 
        }
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true, Negative = true)]
        public uint iprg_no { get; set; }
        [SearchableAttribute(Equality = true)]
        public IprgStateEnum irpg_state { get; set; }
        [SearchableAttribute(Equality = true)]
        public IprgTypeEnum irpg_type { get; set; }
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true, Negative = true)]
        public long last_discovered 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._last_discovered);
            }
            set
            {
                this._last_discovered = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string mac_address 
        {
            get
            {
                return this._mac;
            }
            set
            {
                NetworkAddressTest.IsMAC(value, out this._mac, true, true);
            }
        }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string mgmt_ip_address 
        {
            get
            {
                return this._mgmt_ip_address;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    this._mgmt_ip_address = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The provided management IP address is not valid.");
                }
        
            }
        }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string netbios_name 
        {
            get
            {
                return this._netbios_name;
            }
            set
            {
                if (value.Length <= 15)
                {
                    this._netbios_name = value;
                }
                else
                {
                    throw new ArgumentException("The netbios name must be 15 characters or less.");
                }
            }
        }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string network_component_description { get; set; }
        [SearchableAttribute(Equality = true, Regex = true)]
        public string network_component_ip 
        {
            get
            {
                return this._network_component_ip;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    this._network_component_ip = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The provided network component IP address is not valid.");
                }
            }
        }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string network_component_model { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string network_component_name { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string network_component_port_description { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string network_component_port_name { get; set; }
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true, Negative = true)]
        public string network_component_port_number { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string network_component_type { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string network_component_vendor { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string open_ports { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string os { get; set; }
        [SearchableAttribute(Equality = true)]
        public string port_duplex { get; set; }
        [SearchableAttribute(Equality = true)]
        public string port_link_status { get; set; }
        [SearchableAttribute(Equality = true)]
        public string port_speed { get; set; }
        [SearchableAttribute(Equality = true)]
        public string port_status { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string port_type { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string port_vlan_description { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string port_vlan_name { get; set; }
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true, Negative = true)]
        public string port_vlan_number { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string v_adapter { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string v_cluster { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string v_datacenter { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string v_entity_name { get; set; }
        [SearchableAttribute(Equality = true)]
        public string v_entity_type { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string v_host { get; set; }
        [SearchableAttribute(Equality = true, Regex = true, CaseInsensitive = true)]
        public string v_switch { get; set; }
    }
}
