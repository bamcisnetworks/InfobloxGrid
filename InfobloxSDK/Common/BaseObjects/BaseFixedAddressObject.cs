using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs;
using BAMCIS.Infoblox.Common.InfobloxStructs.Discovery;
using BAMCIS.Infoblox.Common.InfobloxStructs.Grid.CloudApi;
using System;

namespace BAMCIS.Infoblox.Common.BaseObjects
{
    public abstract class BaseFixedAddressObject : BaseNameCommentObject
    {
        private string _device_description;
        private string _device_location;
        private string _device_type;
        private string _device_vendor;
        private string _name;

        public bool allow_telnet { get; set; }
        public clicredential[] cli_credentials { get; set; }
        [ReadOnlyAttribute]
        public info cloud_info { get; internal protected set; }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string device_description
        {
            get
            {
                return this._device_description;
            }
            set
            {
                this._device_description = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string device_location
        {
            get
            {
                return this._device_location;
            }
            set
            {
                this._device_location = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string device_type
        {
            get
            {
                return this._device_type;
            }
            set
            {
                this._device_type = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string device_vendor
        {
            get
            {
                return this._device_vendor;
            }
            set
            {
                this._device_vendor = value.TrimValue();
            }
        }
        public bool disable { get; set; }
        public bool disable_discovery { get; set; }
        [ReadOnlyAttribute]
        public DiscoverNowStatusEnum discover_now_status { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(ContainsSearchable = true)]
        public discoverydata discovered_data { get; internal protected set; }
        [NotReadableAttribute]
        public bool enable_immediate_discovery { get; set; }
        public override string name 
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.TrimValue();
            }
        }
        [SearchableAttribute(Equality = true)]
        public string network_view { get; set; }
        public dhcpoption[] options { get; set; }
        [NotReadableAttribute]
        public bool restart_if_needed { get; set; }
        public snmp3credential snmp3_credential { get; set; }
        public snmpcredential snmp_credential { get; set; }
        [NotReadableAttribute]
        public string template { get; set; }
        public bool use_cli_credentials { get; set; }
        public bool use_options { get; set; }
        public bool use_snmp3_credential { get; set; }
        public bool use_snmp_credential { get; set; }

        public BaseFixedAddressObject()
        {
            this.allow_telnet = false;
            this.cli_credentials = new clicredential[0];
            this.comment = String.Empty;
            this.device_description = String.Empty;
            this.device_location = String.Empty;
            this.device_type = String.Empty;
            this.device_vendor = String.Empty;
            this.disable = false;
            this.disable_discovery = false;
            this.name = String.Empty;
            this.network_view = "default";
            this.options = dhcpoption.DefaultArray();
            this.restart_if_needed = false;
            this.template = String.Empty;
            this.use_cli_credentials = false;
            this.use_options = false;
            this.use_snmp_credential = false;
            this.use_snmp3_credential = false;
        }
    }
}
