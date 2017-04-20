using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:cname")]
    public class cname : BaseRecord
    {
        private string _canonical;
        private string _dns_canonical;

        [Required]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string canonical
        {
            get
            {
                return this._canonical;
            }
            set
            {
                NetworkAddressTest.IsFqdnWithException(value, "canonical", out this._canonical);
            }
        }
        [ReadOnlyAttribute]
        public string dns_canonical 
        {
            get
            {
                return this._dns_canonical;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdnWithException(value, "dns_canonical", out this._dns_canonical);
            }
        }

        public cname(string canonical, string name)
        {
            this.canonical = canonical;
            base.name = name;
        }
    }
}
