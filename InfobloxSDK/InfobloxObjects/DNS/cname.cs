using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;

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
                NetworkAddressTest.IsFqdn(value, "canonical", out this._canonical, false, true);
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
                NetworkAddressTest.IsFqdn(value, "dns_canonical", out this._dns_canonical, false, true);
            }
        }

        public cname(string canonical, string name)
        {
            this.canonical = canonical;
            base.name = name;
        }
    }
}
