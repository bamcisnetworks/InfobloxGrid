using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("member:dns")]
    public class dnsmember
    {
        private string _dtc_health_source_address;
        private string _host_name;
        private string _ipv4addr;
        private string _ipv6addr;

        public DtcHealthSourceEnum dtc_health_source { get; set; }

        public string dtc_health_source_address
        {
            get
            {
                return this._dtc_health_source_address;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv4Address(value, out IP, true, true))
                {
                    this._dtc_health_source_address = IP.ToString();
                }
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string host_name
        {
            get
            {
                return this._host_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "host_name", out this._host_name, true, true);
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv4Address(value, out IP, true, true))
                {
                    this._ipv4addr = IP.ToString();
                }
            }
        }
        
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string ipv6addr
        {
            get
            {
                return this._ipv6addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv6Address(value, out IP, true, true))
                {
                    this._ipv6addr = IP.ToString();
                }
            }
        }

        public loggingcategories logging_categories { get; set; }

        public dnsmember()
        {
            this.dtc_health_source = DtcHealthSourceEnum.VIP;
            this.logging_categories = new loggingcategories() { log_dtc_gslb = false, log_dtc_health = false };
        }
    }
}
