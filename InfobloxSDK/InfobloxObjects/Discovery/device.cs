using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;
using System;
using System.Collections.Generic;
using BAMCIS.Infoblox.InfobloxObjects.DHCP;

namespace BAMCIS.Infoblox.InfobloxObjects.Discovery
{
    [Name("discovery:device")]
    public class device : BaseDiscoveryObject
    {
        private string _address;
        private string _cap_net_deprovisioning_na_reason;
        private string _cap_net_provisioning_na_reason;
        private string _cap_net_vlan_provisioning_na_reason;
        private string _description;
        private string _location;
        private string _model;
        private List<object> _networks;
        private string _os_version;
        private string _vendor;

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
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
        [ReadOnlyAttribute]
        public string address_ref { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool cap_net_deprovisioning_ind { get; internal protected set; }
        [ReadOnlyAttribute]
        public string cap_net_deprovisioning_na_reason
        {
            get
            {
                return this._cap_net_deprovisioning_na_reason;
            }
            internal protected set
            {
                this._cap_net_deprovisioning_na_reason = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public bool cap_net_provisioning_ind { get; internal protected set; }
        [ReadOnlyAttribute]
        public string cap_net_provisioning_na_reason
        {
            get
            {
                return this._cap_net_provisioning_na_reason;
            }
            internal protected set
            {
                this._cap_net_provisioning_na_reason = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public bool cap_net_vlan_provisioning_ind { get; internal protected set; }
        [ReadOnlyAttribute]
        public string cap_net_vlan_provisioning_na_reason
        {
            get
            {
                return this._cap_net_vlan_provisioning_na_reason;
            }
            internal protected set
            {
                this._cap_net_vlan_provisioning_na_reason = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
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
        public deviceinterface[] interfaces { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string location 
        {
            get
            {
                return this._location;
            }
            internal protected set
            {
                this._location = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string model 
        { 
            get
            {
                return this._model;
            }
            internal protected set
            {
                this._model = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public deviceneighbor[] neighbors { get; internal protected set; }
        [ReadOnlyAttribute]
        public string network { get; internal protected set; }
        [ReadOnlyAttribute]
        public networkinfo[] network_infos { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string network_view { get; internal protected set; }
        [ReadOnlyAttribute]
        public object[] networks 
        { 
            get
            {
                return this._networks.ToArray();
            }
            internal protected set
            {
                this._networks = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(ipv4network), typeof(ipv6network) }, value, out this._networks);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality =true, Regex = true)]
        public string os_version 
        { 
            get
            {
                return this._os_version;
            }
            internal protected set
            {
                this._os_version = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public portstatistics port_stats { get; set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string vendor 
        { 
            get
            {
                return this._vendor;
            }
            internal protected set
            {
                this._vendor = value.TrimValue();
            }
        }

    }
}
