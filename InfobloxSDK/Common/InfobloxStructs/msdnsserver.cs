using System;
using System.Net;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class msdnsserver
    {
        private string _ns_ip;
        private string _ns_name;

        public string address { get; set; }
        public bool is_master { get; set; }
        public string ns_ip
        {
            get
            {
                return this._ns_ip;
            }
            set 
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    this._ns_ip = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The ns_ip property must be a valid IP address.");
                }
            }
        }
        public string ns_name
        {
            get
            {
                return this._ns_name;
            }
            set
            {
                this._ns_name = value.TrimValue();
            }
        }
        public bool shared_with_ms_parent_delegation { get; set; }
        public bool stealth { get; set; }

        public msdnsserver()
        {
            this.is_master = false;
            this.stealth = false;
        }
    }
}
