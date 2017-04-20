using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class dhcpoption
    {
        private string _name;
        private string _value;
        private string _vendor_class;

        public string name
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

        public uint num { get; set; }
        public bool use_option { get; set; }

        public string value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value.TrimValueWithException("value");
            }
        }

        public string vendor_class
        {
            get
            {
                return this._vendor_class;
            }
            set
            {
                this._vendor_class = value.TrimValue();
            }
        }

        public dhcpoption()
        {
            use_option = true;
            vendor_class = "DHCP";
        }

        public static dhcpoption DHCPLeaseTime()
        {
            return new dhcpoption() { name = "dhcp-lease-time", num = 51, use_option = false, value = "43200", vendor_class = "DHCP" };
        }

        public static dhcpoption[] DefaultArray()
        {
            return new dhcpoption[] { dhcpoption.DHCPLeaseTime() };
        }
    }
}
