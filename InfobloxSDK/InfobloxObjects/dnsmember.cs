using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs.Grid;

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
                NetworkAddressTest.isIPv4WithExceptionAllowEmpty(value, out this._dtc_health_source_address);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string host_name
        {
            get
            {
                return this._host_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdnWithExceptionAllowEmpty(value, "host_name", out this._host_name);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                NetworkAddressTest.isIPv4WithExceptionAllowEmpty(value, out this._ipv4addr);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv6addr
        {
            get
            {
                return this._ipv6addr;
            }
            set
            {
                NetworkAddressTest.isIPv6WithExceptionAllowEmpty(value, out this._ipv6addr);
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
