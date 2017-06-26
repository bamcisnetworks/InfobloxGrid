using BAMCIS.Infoblox.Core;
using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using BAMCIS.Infoblox.InfobloxObjects.DHCP;
using BAMCIS.Infoblox.InfobloxObjects.DNS;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid.CloudApi
{
    [Name("grid:cloudapi:vmaddress")]
    public class vmaddress : RefObject
    {
        private string _address;
        private List<object> _associated_objects;
        private List<string> _dns_names;
        private bool _is_ipv4;
        private string _mac_address;
        private string _network;

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string address 
        {
            get
            {
                return this._address;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPAddress(value, out this._address, false, true);
            }
        }

        public object[] associated_objects 
        {
            get
            {
                return this._associated_objects.ToArray();
            }
            set
            {
                this._associated_objects = new List<object>();

                ValidateUnknownArray.ValidateHetergenousArray(new List<Type>() {typeof(fixedaddress), typeof(ipv6fixedaddress), typeof(ipv6range),
                        typeof(lease), typeof(a), typeof(aaaa), typeof(host), typeof(host_ipv4addr), typeof(host_ipv6addr), typeof(ptr)  }, value, out this._associated_objects);
            }
        }

        [ReadOnlyAttribute]
        public string[] dns_names 
        {
            get
            {
                return this._dns_names.ToArray();
            }
            internal protected set
            {
                this._dns_names = new List<string>();
                foreach (string item in value)
                {
                    string temp = String.Empty;
                    NetworkAddressTest.IsFqdn(item, "dns_names", out temp, false, true);
                    this._dns_names.Add(temp);
                }
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public bool is_ipv4 
        { 
            get
            {
                return (IPAddress.Parse(this.address).AddressFamily == AddressFamily.InterNetwork);
            }
            internal protected set
            {
                this._is_ipv4 = value;
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string mac_address 
        {
            get
            {
                return this._mac_address;
            }
            internal protected set
            {
                NetworkAddressTest.IsMAC(value, out this._mac_address, true, true);
            }
        }

        [ReadOnlyAttribute]
        public string network 
        { 
            get
            {
                return this._network;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPv4Cidr(value, out this._network, true, true);
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public string network_view { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public uint port_id { get; internal protected set; }

        [ReadOnlyAttribute]
        public string tenant { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string vm_id { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string vm_name { get; internal protected set; }
    }
}
